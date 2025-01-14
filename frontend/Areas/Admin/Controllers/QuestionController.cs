using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Services;
using Placement_Preparation.Utils;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        private readonly string _apiBaseUrl = "Question";
        private readonly ApiClientService _apiClient;
        private readonly AllDropDown _allDropDown;
        private readonly ExportService _exportService;
        public QuestionController(ApiClientService apiClient , AllDropDown allDropDown , ExportService exportService)
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
            _exportService = exportService;
        }

        #region list of Question
        [MainAdminAccess]
        public async Task<IActionResult> ListQuestion()
        {
            ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<QuestionModel>  questionList =  JsonConvert.DeserializeObject<List<QuestionModel>>(response.Data!.ToString());

                 // set search dropdown value ( course , topic , subTopic)
                await  setCourseTopicSubTopicDropDownsValues();

            return View(questionList);
        }
        #endregion

        #region List of Interview Question
        [BothAdminAccess]
        public async Task<IActionResult> InterviewQuestionList()
        {

            string? userId = CV.GetIsAdmin() == null || CV.GetIsAdmin() == true ? null : CV.GetId();
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/InterviewQuestions?addedById={userId}");
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                   ViewData["InternalServerError"] = response.Message;
                   return View();
                }
                List<QuestionModel>  questionList =  JsonConvert.DeserializeObject<List<QuestionModel>>(response.Data!.ToString());
            return View(questionList);
        }
        #endregion

        #region add or Edit Question
        [MainAdminAccess]
        public async Task<IActionResult> AddOrEditQuestion(string? questionId)
        {
            // Set DropDown Value
            await setDropDownsValue();

            // If sub questionId is null then it is add Topic
            if(questionId == null)
            {
                return View();
            }
           
                // Call API to get data
                ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{questionId}");
                if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("ListQuestion");
                }
                QuestionModel question = JsonConvert.DeserializeObject<QuestionModel>(response.Data!.ToString());

                // set topic and sunTopic dropdown
                ViewBag.TopicList = await _allDropDown.GetAllTopicsByCourseId(question.CourseId.ToString());
                ViewBag.SubTopicList = await _allDropDown.GetAllSubTopicsByTopicId(question.TopicId.ToString());

                return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditQuestion(QuestionModel questions)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                 ApiResponseModel response = new ApiResponseModel();
                if(questions.QuestionId != Guid.Empty && questions.QuestionId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, questions);
                }else{
                     // Call API to save data
                     questions.QuestionId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, questions);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("ListQuestion");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }

            // If model state is invalid then set drop down value and return to view
            await setDropDownsValue();

            // set topic dropdown based on course id
            if(questions.CourseId != Guid.Empty)
            {
                ViewBag.TopicList = await _allDropDown.GetAllTopicsByCourseId(questions.CourseId.ToString());
            }

            //set subtopic dropdown based on topic id
            if(questions.TopicId != Guid.Empty){
                ViewBag.SubTopicList = await _allDropDown.GetAllSubTopicsByTopicId(questions.TopicId.ToString());
            }

            return View(questions);
        }
        #endregion
       
        #region Add or Edit Interview Question
        [BothAdminAccess]
        public async Task<IActionResult> AddOrEditInterviewQuestion(string? questionId)
        {
                return View();
        }

        [BothAdminAccess]
        [HttpPost]
        public async Task<IActionResult> AddOrEditInterviewQuestion(QuestionModel questions)
        {
            //Server side validation
            if (ModelState.IsValid){
                ApiResponseModel response = new ApiResponseModel();
                if(questions.QuestionId != Guid.Empty && questions.QuestionId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, questions);
                }else{
                     // Call API to save data
                     questions.QuestionId = Guid.NewGuid();
                     // set status  accept when admin add question
                     if(CV.GetIsAdmin()?? false)
                     {
                         questions.ApproveStatus = "Accept";
                     }
                     response = await _apiClient.PostAsync(_apiBaseUrl, questions);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("InterviewQuestionList");
                }else {
                     TempData["LableErrorMesssage"] = response.Message;
                }
            }
                return View(questions);
        }
        #endregion

        #region delete Question
        [Route("/DeleteQuestion/{questionId}")]
        [MainAdminAccess]
        public async Task<IActionResult> DeleteTopic(string questionId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{questionId}");
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListQuestion");
        }
        #endregion

        #region  Delete Multiple Question
        [MainAdminAccess]
        public async Task<IActionResult> DeleteMultipleQuestion(string  questionIds)
        {
            ApiResponseModel response = await _apiClient.DeleteMultipleAsync($"{_apiBaseUrl}/DeleteMultiple",questionIds.Split(","));
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListQuestion");
        }
        #endregion

        #region set Topic and Sub Topic DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            ViewBag.CourseList = await _allDropDown.Course();
            ViewBag.DifficultyLevelList =await  _allDropDown.DifficultyLevel();
        }
        #endregion
        
         #region set Course , Topic and Sub Topic DropDown Value
        [NonAction]
        public async Task setCourseTopicSubTopicDropDownsValues(){
            ViewBag.CourseList = await _allDropDown.Course();
            ViewBag.TopicList = await _allDropDown.Topic();
            ViewBag.SubTopicList = await _allDropDown.SubTopic();
        }
        #endregion

          #region Get Questions By courseId , topicId and subTopicId
        [HttpGet]
        [Route("/Question/GetQuestions")]
        public async Task<JsonResult> GetMcqsByCourseIdTopicIdSubTopicId([FromQuery] Guid? courseId ,[FromQuery] Guid? topicId ,[FromQuery] Guid? subTopicId , [FromQuery] bool? onlyActiveQuestions=false)
        {
             string url = onlyActiveQuestions == true ? $"{_apiBaseUrl}?courseId={courseId}&topicId={topicId}&subTopicId={subTopicId}&onlyActiveQuestions=true" : $"{_apiBaseUrl}?courseId={courseId}&topicId={topicId}&subTopicId={subTopicId}";
            ApiResponseModel response = await _apiClient.GetAsync(url);if(response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return new JsonResult(null);
            }
            List<QuestionModel> questionList = JsonConvert.DeserializeObject<List<QuestionModel>>(response.Data!.ToString());
            return new JsonResult(questionList);
        }
        #endregion

        #region Export Question to Excel
        public async Task<IActionResult> ExportToExcelQuestion()
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}");
            if(response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ListQuestion");
            }

            try
            {
                 // Ensure response.Data is cast to IEnumerable<QuestionModel>
                var questionList = JsonConvert.DeserializeObject<List<QuestionModel>>(response.Data!.ToString()) as IEnumerable<QuestionModel>;
                byte[] fileContents = _exportService.ExportToExcel(questionList, "Question");

                // Return as a file to trigger download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Question.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListQuestion");
            }
        }
        #endregion
   
        #region  clear search
        public IActionResult ClearSearch()
        {
            return RedirectToAction("ListQuestion");
        }
        #endregion



    }
}


