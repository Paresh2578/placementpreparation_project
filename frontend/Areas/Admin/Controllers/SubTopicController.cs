using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Data.Interface;
using Placement_Preparation.Areas.Admin.Models;
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
        public SubTopicController(ApiClientService apiClient , TopicInterface topicInterface , AllDropDown allDropDown , SubTopicInterface subTopicInterface)
        {
            _apiClient = apiClient;
            _topicInterface = topicInterface;
            _allDropDown = allDropDown;
            _subTopicInterface = subTopicInterface;
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

        #region Get Sub Topics By Topic Id for set Sub Topic DropDown Value
        [HttpGet]
        [Route("/GetSubTopicsByTopicId/{topicId}")]
        public async Task<JsonResult> GetSubTopicsByTopicId(string topicId)
        {
            JsonResult jsonSubTopicList = await _subTopicInterface.GetSubTopicsByTopicId(topicId);
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


        
    }
}
