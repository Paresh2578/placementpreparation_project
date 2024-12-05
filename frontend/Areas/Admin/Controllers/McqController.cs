using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Services;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class McqController : Controller
    {
        private readonly string _apiBaseUrl = "Mcq";
        private readonly ApiClientService _apiClient;
        private readonly AllDropDown _allDropDown;
        private readonly ExportService _exportService;
        public McqController(ApiClientService apiClient , AllDropDown allDropDown , ExportService exportService)
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
            _exportService = exportService;
        }

        #region list of Mcq
        public async Task<IActionResult> ListMcq(Guid? courseId , Guid? topicId , Guid? subTopicId)
        {
             ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}?courseId={courseId}&topicId={topicId}+&subTopicId="
             +subTopicId);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<McqModel>  mcqList =  JsonConvert.DeserializeObject<List<McqModel>>(response.Data!.ToString());

                // set search dropdown value ( course , topic , subTopic)
                await  setCourseTopicSubTopicDropDownsValues();

            return View(mcqList);
        }
        #endregion

        #region add or Edit Mcq
        public async Task<IActionResult> AddOrEditMcq(string? mcqId)
        {
           // Set DropDown Value
            await setDropDownsValue();

            // If sub mcqId is null then it is add Topic
            if(mcqId == null)
            {
                return View();
            }
           
                // Call API to get data
                ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{mcqId}");
                if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("ListMcq");
                }
                McqModel mcq = JsonConvert.DeserializeObject<McqModel>(response.Data!.ToString());

                 // set topic and sunTopic dropdown
                ViewBag.TopicList = await _allDropDown.GetAllTopicsByCourseId(mcq.CourseId.ToString());
                ViewBag.SubTopicList = await _allDropDown.GetAllSubTopicsByTopicId(mcq.TopicId.ToString());

                return View(mcq);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditMcq(McqModel mcq)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                 ApiResponseModel response = new ApiResponseModel();
                if(mcq.McqId != Guid.Empty && mcq.McqId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, mcq);
                }else{
                     // Call API to save data
                     mcq.McqId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, mcq);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("ListMcq");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }

            // If model state is invalid then set drop down value and return to view
            await setDropDownsValue();

            // set topic dropdown based on course id
            if(mcq.CourseId != Guid.Empty)
            {
                ViewBag.TopicList = await _allDropDown.GetAllTopicsByCourseId(mcq.CourseId.ToString());
            }

            //set subtopic dropdown based on topic id
            if(mcq.TopicId != Guid.Empty){
                ViewBag.SubTopicList = await _allDropDown.GetAllSubTopicsByTopicId(mcq.TopicId.ToString());
            }

            return View(mcq);
        }
        #endregion

        #region delete Mcq
        [Route("/DeleteMcq/{mcqId}")]
        public async Task<IActionResult> DeleteTopic(string mcqId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{mcqId}");
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListMcq");
        }
        #endregion

        #region set Topic and Sub Topic DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            ViewBag.CourseList = await _allDropDown.Course();
            ViewBag.DifficultyLevelList =await _allDropDown.DifficultyLevel();
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
   
         #region Export Mcq to Excel
        public async Task<IActionResult> ExportToExcelMcq()
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}");
            if(response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ListMcq");
            }

            try
            {
                 // Ensure response.Data is cast to IEnumerable<McqModel>
                var mcqList = JsonConvert.DeserializeObject<List<McqModel>>(response.Data!.ToString()) as IEnumerable<McqModel>;
                byte[] fileContents = _exportService.ExportToExcel(mcqList, "Mcq");

                // Return as a file to trigger download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mcq.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListMCq");
            }

        //     try
        //     {
        //         // Ensure response.Data is cast to IEnumerable<McqModel>
        //         List<McqModel> mcqList = JsonConvert.DeserializeObject<List<McqModel>>(response.Data!.ToString());

        //         List<string> columns = ["subTopicId","subTopicName","content","topicId","topicName"];

        //         List<Dictionary<string,dynamic>> mcqObeData = new List<Dictionary<string,dynamic>> { };

        //         foreach(McqModel mcq in mcqList)
        //         {

        //             // Create a flattened dictionary
        //             var flattenedObject = new Dictionary<string, dynamic>
        //             {
        //                 { "mcqId", mcq.McqId.ToString() ?? "N/A" },
        //                 { "questionText", mcq.QuestionText ?? "N/A" },
        //                 { "optionA", mcq.OptionA ?? "N/A" },
        //                 { "optionB", mcq.OptionB ?? "N/A" },
        //                 { "optionC", mcq.OptionC ?? "N/A" }, 
        //                 { "optionD", mcq.OptionD ?? "N/A" }, 
        //                 { "correctAnswer", mcq.CorrectAnswer ?? "N/A" }, 
        //                 { "answerDescription", mcq.AnswerDescription ?? "N/A" }, 
        //                 { "isActive", mcq.IsActive.ToString() ?? "N/A" }, 
        //                 { "courseId", mcq.CourseId.ToString() ?? "N/A" }, 
        //                 { "topicId", mcq.TopicId.ToString() ?? "N/A" }, 
        //                 { "subTopicId", mcq.SubTopicId.ToString() ?? "N/A" }, 
        //                 { "difficultyLevelId", mcq.DifficultyLevelId.ToString() ?? "N/A" },
        //             };
        //             mcqObeData.Add(flattenedObject);
        //         }

        //         // Call ExportToExcelBySpecificColumn method
        //         byte[] fileContents = _exportService.ExportToExcelBySpecificColumn(mcqObeData, columns.ToArray());

        //         // Return as a file to trigger download
        //         return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mcq.xlsx");
        //     }
        //     catch (Exception ex)
        //     {
        //         TempData["ErrorMessage"] = ex.Message;
        //         return RedirectToAction("ListMcq");
        //     }
        // 
        }
        #endregion
   
    }
}
