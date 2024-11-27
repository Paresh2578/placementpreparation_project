using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubTopicController : Controller
    {
        private readonly string _apiBaseUrl = "SubTopic";
        private readonly ApiClientService _apiClient;
        public SubTopicController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        #region list of Sub Topic
        public async Task<IActionResult> ListSubTopic()
        {
           ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<SubTopicModel>  subTopicList =  JsonConvert.DeserializeObject<List<SubTopicModel>>(response.Data!.ToString());

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

        #region set Topic DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            AllDropDown AllDropDown = new AllDropDown(_apiClient);
            ViewBag.courseList = await AllDropDown.Course();
            ViewBag.topicList = await AllDropDown.Topic();
            ViewBag.difficultyLevelList = await AllDropDown.DifficultyLevel();
        }
        #endregion
    }
}
