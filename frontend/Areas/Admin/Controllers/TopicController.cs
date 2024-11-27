using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicController : Controller
    {
        private readonly string _apiBaseUrl = "Topic";
        private readonly ApiClientService _apiClient;
        public TopicController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        #region list of Topic
        public async Task<IActionResult> ListTopic()
        {
             ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<TopicModel>  topicList =  JsonConvert.DeserializeObject<List<TopicModel>>(response.Data!.ToString());

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

        #region set Course  and Difficulty Level DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            AllDropDown AllDropDown = new AllDropDown(_apiClient);
            ViewBag.CourseList = await AllDropDown.Course();
            ViewBag.difficultyLevelList = await AllDropDown.DifficultyLevel();
        }
        #endregion
    }
}
