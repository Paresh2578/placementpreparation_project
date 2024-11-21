using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckAccess]
    public class BranchController : Controller
    {
        private readonly ApiClientService _apiClient;
        private readonly string _apiBaseUrl = "Branch";
        public BranchController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }
        

        #region list of Branch
        public async Task<IActionResult> List()
        {
                ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<BranchModel>  branchList =  JsonConvert.DeserializeObject<List<BranchModel>>(response.Data!.ToString());

            return View(branchList);
        }
        #endregion


        #region add or Edit Branch
        [HttpGet]
        public async Task<IActionResult> AddOrEditBranch(string? branchId)
        {
            // If branchId is null then it is add branch
            if(branchId == null)
            {
                return View();
            }
           
                // Call API to get data
                ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{branchId}");
                Console.WriteLine("response data " + response.Data);
                if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("List");
                }
                BranchModel branch = JsonConvert.DeserializeObject<BranchModel>(response.Data!.ToString());
                return View(branch);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditBranch(BranchModel branch)
        {

            //Server side validation
            if (ModelState.IsValid)
            {
                ApiResponseModel response = new ApiResponseModel();
                if(branch.BranchId != Guid.Empty && branch.BranchId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, branch);
                }else{
                     // Call API to save data
                     branch.BranchId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, branch);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("List");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }
            return View(branch);
        }
        #endregion
         
        #region Delete Branch
        [Route("Admin/Branch/DeleteBranch/{branchId}")]
        public async Task<IActionResult> DeleteBranch(string branchId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{branchId}");
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
