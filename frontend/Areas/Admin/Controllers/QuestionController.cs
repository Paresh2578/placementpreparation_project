using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        private readonly string _apiBaseUrl = "Question";
        private readonly ApiClientService _apiClient;
        private readonly AllDropDown _allDropDown;
        public QuestionController(ApiClientService apiClient , AllDropDown allDropDown)
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
        }

        #region list of Topic
        public async Task<IActionResult> ListQuestion()
        {
            ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<QuestionModel>  questionList =  JsonConvert.DeserializeObject<List<QuestionModel>>(response.Data!.ToString());

            return View(questionList);
        }
        #endregion

        #region add or Edit Question
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

        
        #region delete Question
        [Route("/DeleteQuestion/{questionId}")]
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

        #region set Topic and Sub Topic DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            // AllDropDown AllDropDown = new AllDropDown(_apiClient);
            ViewBag.CourseList = await _allDropDown.Course();
            // ViewBag.TopicList = await _allDropDown.Topic();
            // ViewBag.SubTopicList =await  _allDropDown.SubTopic();
            ViewBag.DifficultyLevelList =await  _allDropDown.DifficultyLevel();
        }
        #endregion
    }
}
