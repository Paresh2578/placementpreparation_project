using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [MainAdminAccess]
    public class FeedbackController : Controller
    { 
        private readonly ApiClientService _apiClientService;
        private readonly string _baseUrl = "Feedback";
        public FeedbackController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }
        public async Task<IActionResult> ListFeedback(){
            ApiResponseModel response = await _apiClientService.GetAsync(_baseUrl);
            List<FeedbackModel> feedbacks = new List<FeedbackModel>();
            if(response.StatusCode != 200){
                 TempData["ErrorMessage"] = response.Message;
                   ViewData["InternalServerError"] = response.Message;
                   return View(feedbacks);
            }

             feedbacks = JsonConvert.DeserializeObject<List<FeedbackModel>>(response.Data.ToString());
            return View(feedbacks);
        }
    }
}