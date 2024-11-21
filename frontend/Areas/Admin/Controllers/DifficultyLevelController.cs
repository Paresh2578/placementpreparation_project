using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DifficultyLevelController : Controller
    {
         private readonly string _apiBaseUrl = "DifficultyLevel";
        private readonly ApiClientService _apiClient;

        public DifficultyLevelController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        #region list of Difficulty Level 
        public async Task<IActionResult> List()
        {
            ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<DifficultyLevelModel>  difficultyLevelList =  JsonConvert.DeserializeObject<List<DifficultyLevelModel>>(response.Data!.ToString());

           
            return View(difficultyLevelList);
        }
        #endregion


        #region add or Edit Branch
        public async Task<IActionResult> AddOrEditDifficultyLevel(string? difficultyLevelId)
        {
            // If DifficultyLevelId is null then it is add DifficultyLevel
            if(difficultyLevelId == null)
            {
                return View();
            }
           
                // Call API to get data
                ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{difficultyLevelId}");
                if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("List");
                }
                DifficultyLevelModel difficultyLevel = JsonConvert.DeserializeObject<DifficultyLevelModel>(response.Data!.ToString());
                return View(difficultyLevel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditDifficultyLevel(DifficultyLevelModel difficultyLevel)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                 ApiResponseModel response = new ApiResponseModel();
                if(difficultyLevel.DifficultyLevelId != Guid.Empty && difficultyLevel.DifficultyLevelId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, difficultyLevel);
                }else{
                     // Call API to save data
                     difficultyLevel.DifficultyLevelId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, difficultyLevel);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("List");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }
            return View(difficultyLevel);
        }
        #endregion
   
         #region Delete DifficultyLevel
        [Route("/DeleteDifficultyLevel/{difficultyLevelId}")]
        public async Task<IActionResult> DeleteDifficultyLevel(string difficultyLevelId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{difficultyLevelId}");
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else{
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("List");
        }
        #endregion
    }
}
