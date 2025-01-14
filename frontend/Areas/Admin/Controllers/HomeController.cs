using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [BothAdminAccess]
    public class HomeController : Controller
    {
        private readonly ApiClientService _apiClientService;
        private readonly string baseUrl = "Home";
        public HomeController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        #region  Dashboard data Show
        public async Task<IActionResult> Home()
        {
            ApiResponseModel response =await _apiClientService.GetAsync($"{baseUrl}/GetDashboardData");
            if(response.StatusCode != 200)
            {
                ViewData["InternalServerError"] = response.Message;
                return View(new DashboardModel());
            }
            DashboardModel dashboardModel = JsonConvert.DeserializeObject<DashboardModel>(response.Data.ToString());
            
            return View(dashboardModel);
        }
        #endregion

        #region  Update Approvel StudentStatus
        [HttpGet("UpdateApprovelStudentStatus/{id}/{status}")]
        public async Task<IActionResult> UpdateStudentApprovelStatus(string id , string status)
        {
            ApiResponseModel response = await _apiClientService.PutAsync($"AdminUser/UpdateApprovelStudentStatus/{id}/{status}", "");

            if (response.StatusCode != 200)
            {
                ViewData["LableErrorMesssage"] = response.Message;
            }
            return RedirectToAction("Home");
        }
        #endregion

         #region  Update New Interview Question Request Status
        [HttpGet("UpdateNewInterviewQuestionRequestStatus/{id}/{status}")]
        public async Task<IActionResult> UpdateNewInterviewQuestionRequestStatus(string id , string status)
        {
            ApiResponseModel response = await _apiClientService.PutAsync($"Question/UpdateNewInterviewQuestionRequestStatus/{id}/{status}", "");

            if (response.StatusCode != 200)
            {
                ViewData["LableErrorMesssage"] = response.Message;
            }
            return RedirectToAction("Home");
        }
        #endregion

        #region  Update New Interview Question Request Status
        [HttpGet("UpdateNewInterviewMcqRequestStatus/{id}/{status}")]
        public async Task<IActionResult> UpdateNewInterviewMcqRequestStatus(string id , string status)
        {
            ApiResponseModel response = await _apiClientService.PutAsync($"Mcq/UpdateNewInterviewMcqRequestStatus/{id}/{status}", "");

            if (response.StatusCode != 200)
            {
                ViewData["LableErrorMesssage"] = response.Message;
            }
            return RedirectToAction("Home");
        }
        #endregion
    }
}
