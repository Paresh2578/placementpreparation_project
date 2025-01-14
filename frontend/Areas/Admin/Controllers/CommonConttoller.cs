using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Frontend.Services;
using Newtonsoft.Json;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : Controller
    {
        private readonly ApiClientService _apiClientService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonController(ApiClientService apiClientService, IHttpContextAccessor httpContextAccessor)
        {
            _apiClientService = apiClientService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region  Error 403 Forbidden Page
        public IActionResult Error403()
        {
            return View();
        }
        #endregion

        #region  Approval Status Page
        public async Task<IActionResult> ApprovalStatus()
        {
            try{// /api/AdminUser/refreshToken
                ApiResponseModel response = await _apiClientService.PostAsync($"AdminUser/refreshToken/{CV.GetId()}","");
                if(response.StatusCode == 200){
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

                    Dictionary<string, dynamic> userData = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(response.Data.ToString());

                    if (userData["data"]["approveStatus"] != CV.GetApprovalStatus())
                    {
                        return RedirectToAction("Home", "Home");
                    }

                }
            }catch{
            }
            return View();
        }
        #endregion
    }
} 