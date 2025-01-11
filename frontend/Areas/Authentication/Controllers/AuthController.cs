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
        private readonly  IHttpContextAccessor _httpContextAccessor;
        private readonly string _apiBaseUrl = "AdminUser";
        public AuthController(ApiClientService apiClient , IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
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

                    //get the user 
                    
                    string userName = response.Data!.userData.userName;
                    string email = response.Data!.userData.email;


                    // set the user session
                   _httpContextAccessor.HttpContext!.Session.SetString("UserName", userName);
                   _httpContextAccessor.HttpContext.Session.SetString("email", email);

                    // set token in cookie
                    var token = response.Data!.token.ToString() ?? string.Empty;
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true, // Set to true in production
                        SameSite = SameSiteMode.None, // Adjust as needed
                        // Expires = DateTime.Now.AddDays(1)
                        Expires = DateTime.Now.AddHours(1)
                    };

                    _httpContextAccessor.HttpContext!.Response.Cookies.Append("token", token, cookieOptions);

                   // notify the user
                    TempData["SuccessMessage"] = response.Message;

                   //redirect to the home page
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

        #region sign up
        public IActionResult SignUp()
        {
            return View();
        }
        #endregion
        
        #region  Log out
        [Route("~/logout")]
        public IActionResult LogOutAdmin()
        {
            _httpContextAccessor.HttpContext!.Session.Clear();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("token");
            return RedirectToAction("Home", "Home", new { area = "Student" });
        }
        #endregion
    }
}
