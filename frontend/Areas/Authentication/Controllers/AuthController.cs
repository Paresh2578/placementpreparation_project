using Frontend.Areas.Authentication.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Newtonsoft.Json;




namespace Frontend.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    public class AuthController : Controller
    {
        private readonly ApiClientService _apiClient;
        private readonly string _apiBaseUrl = "AdminUser";
        public AuthController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        #region sign in
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/signin/{model.Email}/{model.Password}", model);
                if (response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("Home", "Home", new { area = "Admin" });
                }
                else
                {
                    TempData["ErrorMessage"] = response.Message;
                }
            }
            return View(model);
        }
        #endregion


    }
}
