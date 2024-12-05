using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Data.Interface;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Services;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubTopicController : Controller
    {
        private readonly string _apiBaseUrl = "SubTopic";
        private readonly ApiClientService _apiClient;
        private readonly TopicInterface _topicInterface;
        private readonly SubTopicInterface _subTopicInterface;
        private readonly AllDropDown _allDropDown;
        private readonly ExportService _exportService;
        public SubTopicController(ApiClientService apiClient , TopicInterface topicInterface , AllDropDown allDropDown , SubTopicInterface subTopicInterface,ExportService exportService)
        {
            _apiClient = apiClient;
            _topicInterface = topicInterface;
            _allDropDown = allDropDown;
            _subTopicInterface = subTopicInterface;
            _exportService = exportService;
        }

        #region list of Sub Topic
        public async Task<IActionResult> ListSubTopic()
        {
           ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);

            List<SubTopicModel> subTopicList = new List<SubTopicModel> { };
                if (response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                  subTopicList =  JsonConvert.DeserializeObject<List<SubTopicModel>>(response.Data!.ToString());

            return View(subTopicList);
        }
        #endregion

        #region add or Edit Sub Topic
        public async Task<IActionResult> AddOrEditSubTopic(string? subTopicId)
        {
              // Set DropDown Value
                await setDropDownsValue();

            // If sub topicId is null then it is add Topic
            if(subTopicId == null)
            {
                return View();
            }
           
                // Call API to get data
                ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{subTopicId}");
                if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("ListSubTopic");
                }
                SubTopicModel subTopic = JsonConvert.DeserializeObject<SubTopicModel>(response.Data!.ToString());

                // set Topic Dropdown value
                ViewBag.topicList = await _allDropDown.GetAllTopicsByCourseId(subTopic.CourseId.ToString());

              
                return View(subTopic);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditSubTopic(SubTopicModel subTopic)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                 ApiResponseModel response = new ApiResponseModel();
                if(subTopic.SubTopicId != Guid.Empty && subTopic.SubTopicId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, subTopic);
                }else{
                     // Call API to save data
                     subTopic.SubTopicId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, subTopic);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("ListSubTopic");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }

            // If model state is invalid then set drop down value and return to view
            await setDropDownsValue();

            // set Topic Dropdown value
            if(subTopic.CourseId != Guid.Empty && subTopic.CourseId != null)
            {
                ViewBag.topicList = await _allDropDown.GetAllTopicsByCourseId(subTopic.CourseId.ToString());
            }

            return View(subTopic);
        }
        #endregion

          #region delete Topic
        [Route("/DeleteSubTopic/{subTopicId}")]
        public async Task<IActionResult> DeleteTopic(string subTopicId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{subTopicId}");
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListSubTopic");
        }
        #endregion

        #region Get Sub Topics By Topic Id 
        [HttpGet]
        [Route("/GetSubTopicsByTopicId/{topicId}")]
        public async Task<JsonResult> GetSubTopicsByTopicId(string topicId)
        {
            JsonResult jsonSubTopicList = await _subTopicInterface.GetSubTopicsByTopicId(topicId);
            return Json(jsonSubTopicList);
        }
        #endregion

        #region Get Sub Topics By Topic Id 
        [HttpGet]
        [Route("/GetSubTopicsByCourseId/{CourseId}")]
        public async Task<JsonResult> GetSubTopicsByCourseId(string courseId)
        {
            JsonResult jsonSubTopicList = await _subTopicInterface.GetSubTopicsByCourseId(courseId);
            return Json(jsonSubTopicList);
        }
        #endregion

        #region set Topic DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            ViewBag.courseList = await _allDropDown.Course();
            // ViewBag.topicList = await AllDropDown.Topic();
            ViewBag.difficultyLevelList = await _allDropDown.DifficultyLevel();
        }
        #endregion

         #region Export Sub Topic to Excel
        public async Task<IActionResult> ExportToExcelSubTopic()
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}");
            if(response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ListSubTopic");
            }

            try
            {
                // Ensure response.Data is cast to IEnumerable<SubTopicModel>
                List<SubTopicModel> topicList = JsonConvert.DeserializeObject<List<SubTopicModel>>(response.Data!.ToString());

                List<string> columns = ["subTopicId","subTopicName","content","topicId","topicName"];

                List<Dictionary<string,dynamic>> subTopicObeData = new List<Dictionary<string,dynamic>> { };

                foreach(SubTopicModel subTopic in topicList)
                {

                    // Create a flattened dictionary
                    var flattenedObject = new Dictionary<string, dynamic>
                    {
                        { "subTopicId", subTopic.SubTopicId.ToString() ?? "N/A" },
                        { "subTopicName", subTopic.SubTopicName ?? "N/A" },
                        { "content", subTopic.Content ?? "N/A" },
                        { "topicId", subTopic.TopicId.ToString() ?? "N/A" },
                        { "topicName", subTopic.Topic?.TopicName ?? "N/A" }, 
                    };
                    subTopicObeData.Add(flattenedObject);
                }

                // Call ExportToExcelBySpecificColumn method
                byte[] fileContents = _exportService.ExportToExcelBySpecificColumn(subTopicObeData, columns.ToArray());

                // Return as a file to trigger download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SubTopic.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListSubTopic");
            }
        }

        #endregion
   
    }
}
