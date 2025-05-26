
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Student.Models;
using Placement_Preparation.BAL;


namespace Placement_Preparation.Areas.Student.Controllers
{
    [Area("Student")]
    public class CommunityController : Controller
    {
        private readonly ApiClientService _apiClient;

        public CommunityController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }
        private readonly string _apiBaseUrl = "Community";

        #region List All Questions
        public async Task<IActionResult> ListAllQuestions()
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}");
            if (response.StatusCode != 200)
            {
                ViewData["InternalServerError"] = response.Message;
                return View(new List<PostModel>());
            }
            List<PostModel> questions = JsonConvert.DeserializeObject<List<PostModel>>(response.Data.ToString());
            return View(questions);
        }
        #endregion

        #region Question Details
        public async Task<IActionResult> QuestionDetails(String questionId)
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{questionId}");
            PostModel? question = null;
            if (response.StatusCode != 200)
            {
                ViewData["InternalServerError"] = response.Message;
                return View(question);
            }
            if (response.Data != null)
            {
                question = JsonConvert.DeserializeObject<PostModel>(response.Data.ToString());
            }
            return View(question);
        }
        #endregion

        #region Ask Question
        [BothAdminAccess]
        public async Task<IActionResult> AskQuestion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AskQuestion(PostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return View(postModel);
            }

            ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/AskQuestion", postModel);
            if (response.StatusCode != 201)
            {
                ViewData["InternalServerError"] = response.Message;
                return View(postModel);
            }

            // notify the user
            TempData["SuccessMessage"] = response.Message;

            return RedirectToAction("ListAllQuestions");
        }
        #endregion
    }
}