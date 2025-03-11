using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Data.Interface;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Services;
using Placement_Preparation.Utils;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [MainAdminAccess]
    public class TopicController : Controller
    {
        private readonly string _apiBaseUrl = "Topic";
        private readonly ApiClientService _apiClient;
        private readonly AllDropDown _allDropDown;
        private readonly TopicInterface _topicInterface;
        private readonly ExportService _exportService;
        public TopicController(ApiClientService apiClient , AllDropDown allDropDown , TopicInterface topicInterface,ExportService exportService)    
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
            _topicInterface = topicInterface;
            _exportService = exportService;
        }

        #region list of Topic
        public async Task<IActionResult> ListTopic()
        {
             ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
             List<TopicModel> topicList = new List<TopicModel>();
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                     return View(topicList);
                }
                topicList =  JsonConvert.DeserializeObject<List<TopicModel>>(response.Data!.ToString());

            return View(topicList);
        }
        #endregion

        #region add or Edit Topic
        public async Task<IActionResult> AddOrEditTopic(string? topicId)
        {
            // Set DropDown Value
            await setDropDownsValue();

         // If topicId is null then it is add Topic
            if(topicId == null)
            {
                return View();
            }
           
                // Call API to get data
                ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{topicId}");
                if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("ListTopic");
                }
                TopicModel topic = JsonConvert.DeserializeObject<TopicModel>(response.Data!.ToString());

                // set Level dropdown value
                ViewBag.LevelList = await _allDropDown.GetTopicLengthByCourseId(topic.CourseId.ToString());

                

                return View(topic);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditTopic(TopicModel topic)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                 ApiResponseModel response = new ApiResponseModel();
                if(topic.TopicId != Guid.Empty && topic.TopicId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, topic);
                }else{
                     // Call API to save data
                     topic.TopicId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, topic);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("ListTopic");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }

            // Set DropDown Value
            await setDropDownsValue();

            return View(topic);
        }
        #endregion

        #region delete Topic
        [Route("/DeleteTopic/{topicId}")]
        public async Task<IActionResult> DeleteTopic(string topicId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{topicId}");
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListTopic");
        }
        #endregion
        
           
        #region  Delete Multiple  Topics
        public async Task<IActionResult> DeleteMultipleTopic(string  topicIds)
        {
            ApiResponseModel response = await _apiClient.DeleteMultipleAsync($"{_apiBaseUrl}/DeleteMultiple",topicIds.Split(","));
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListTopic");
        }
        #endregion

        #region set Course  and Difficulty Level DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            // AllDropDown AllDropDown = new AllDropDown(_apiClient);
            ViewBag.CourseList = await _allDropDown.Course();
            ViewBag.difficultyLevelList = await _allDropDown.DifficultyLevel();
        }
        #endregion

                
        #region Get Topics By CourseId for set Topic DropDown Value
        [HttpGet]
        [Route("/GetTopicsByCourseId/{courseId}")]
        public async Task<JsonResult> GetTopicsByCourseId(string courseId)
        {
            JsonResult jsonTopicList = await _topicInterface.GetTopicsByCourseId(courseId:courseId);
            return Json(jsonTopicList);
        }
        #endregion

         #region Export Topic to Excel
        public async Task<IActionResult> ExportToExcelTopic()
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}");
            if(response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ListTopic");
            }

            try
            {
                // Ensure response.Data is cast to IEnumerable<TopicModel>
                List<TopicModel> topicList = JsonConvert.DeserializeObject<List<TopicModel>>(response.Data!.ToString());

                List<string> columns = ["topicId","topicName","courseId","courseName","description","img","content","difficultyLevelId","difficultyLevelName"];

                List<Dictionary<string,dynamic>> topicObeData = new List<Dictionary<string,dynamic>> { };

                foreach(TopicModel topic in topicList)
                {

                    // Create a flattened dictionary
                    var flattenedObject = new Dictionary<string, dynamic>
                    {
                        { "topicId", topic.TopicId.ToString() ?? "N/A" },
                        { "topicName", topic.TopicName },
                        { "courseId", topic.CourseId },
                        { "courseName", topic.Course?.CourseName ?? "N/A" }, // Extract Course from branch
                        { "description", topic.Course?.Description ?? "N/A"},
                        { "img", topic.Course?.Img ?? "N/A"},
                        { "content", topic.Content ?? "N/A" },
                        { "difficultyLevelId", topic.DifficultyLevelId.ToString() ?? "N/A" }, // Extract DifficultyLevel from courseType
                        { "difficultyLevelName", topic.DifficultyLevel?.DifficultyLevelName ?? "N/A" }
                    };
                    topicObeData.Add(flattenedObject);
                }

                // Call ExportToExcelBySpecificColumn method
                byte[] fileContents = _exportService.ExportToExcelBySpecificColumn(topicObeData, columns.ToArray());

                // Return as a file to trigger download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Topic.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListTopic");
            }
        }

        #endregion
        
        #region Get LevelList by courseId
        [Route("/Topic/GetLevelsListByCourseId")]
        public async Task<List<int>> GetLevelsListByCourseId([FromQuery] string courseId){
            JsonResult jsonResult = await _topicInterface.GetTopicsLengthByCourseId(courseId:courseId);

            dynamic jsonValue = jsonResult.Value;
            int n = jsonValue.length;

            List<int> levelList = new List<int>{};
            for(int i=1;i<=n;i++){
                levelList.Add(i);
            }

            return levelList;
        }
        #endregion
    }
}
