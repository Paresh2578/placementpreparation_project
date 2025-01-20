using Frontend.Areas.Authentication.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Newtonsoft.Json;
using Placement_Preparation.Utils;
using Placement_Preparation.BAL;




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
                   CV.SetEmail(email);
                   CV.SetUserName(userName);

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
                    TempData["LableErrorMesssage"] = response.Message;
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

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {

            if (ModelState.IsValid)
            {
                model.Email = CV.GetEmail();
                ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/signup", model);
                if (response.StatusCode == 201)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("SignIn");
                }
                else
                {
                    TempData["LableErrorMesssage"] = response.Message;
                }
            }
            return View(model);
        }

        #endregion

        #region  EmailVarification
        public IActionResult EmailVarification()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailVarification(EmailVarificationModel emailVarification)
        {

            if (ModelState.IsValid)
            {
                ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/sendOtp/{emailVarification.Email}","");
                if (response.StatusCode == 200)
                {
                   TempData["SuccessMessage"] = response.Message;
                // encypt email and send to the otp verification page
                return RedirectToAction("OTPVerification", new { email = UrlEncryptor.Encrypt(emailVarification.Email) });
                }
                else
                {
                   TempData["LableErrorMesssage"] = response.Message;
                }
            }
            return View(emailVarification);
        }
        #endregion

        #region  ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassword forgetPassword)
        {
            if (ModelState.IsValid)
            {
                ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/forgetPassword/{forgetPassword.Email}", "");
                if (response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("SentLinkSuccessfully");
                }
                else
                {
                    TempData["LableErrorMesssage"] = response.Message;
                }
            }
            return View(forgetPassword);
        }
        #endregion

        #region  OTP Verification
        public IActionResult OTPVerification(string email)
        {
            return View(new OTPModel() { Email =email});
        }

        [HttpPost]
        public async Task<IActionResult> OTPVerification(OTPModel otp)
        {
            string otpCode =(otp.FirstDigit == null || otp.SecoundDigit == null || otp.ThirdDigit == null || otp.FourthDigit == null || otp.FifthDigit == null || otp.SixthDigit == null) ? "" : otp.FirstDigit.ToString() + otp.SecoundDigit.ToString() + otp.ThirdDigit.ToString() + otp.FourthDigit.ToString() + otp.FifthDigit.ToString() + otp.SixthDigit.ToString();
            // otp varification
            if(otpCode.Trim().Length != 6)
            {
                ModelState.AddModelError("invalidOtp", "Please enter the OTP");
                TempData["LableErrorMesssage"] = "Please enter the OTP";
                return View(otp);
            }else{
                ModelState.Remove("invalidOtp");
            }

            if (ModelState.IsValid)
            {
                ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/VerifyOtp/{UrlEncryptor.Decrypt(otp.Email)}/{otpCode}", "");
                if (response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    // store email in seesion
                    CV.SetEmail(UrlEncryptor.Decrypt(otp.Email));
                    return RedirectToAction("SignUp");
                }
                else
                {
                    TempData["LableErrorMesssage"] = response.Message;
                }
            }
            return View(otp);
        }

        #endregion


        #region Resend OTP
        [HttpPost]
        public async Task<IActionResult> ResendOtp([FromBody] EmailVarificationModel email)
        {
            try
            {
                if (string.IsNullOrEmpty(email.Email))
                {
                    return Json(new { success = false, message = "Invalid email address." });
                }


                ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/sendOtp/{UrlEncryptor.Decrypt(email.Email)}", "");
                if (response.StatusCode == 200)
                {
                    return Json(new { success = true, message = "OTP has been resent successfully." });
                }

                return Json(new { success = false, message = response.Message });
            }catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region sent link successfully 
        public IActionResult SentLinkSuccessfully()
        {
            return View();
        }
        #endregion

        #region  Reset Password
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordModel() { Token = token,NewPassword = "",ConfirmPassword = "" });
        }
        [HttpPost]

        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                ApiResponseModel response = await _apiClient.PostAsync($"{_apiBaseUrl}/resetPassword", resetPassword);
                if (response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("SignIn");
                }
                else
                {
                    TempData["LableErrorMesssage"] = response.Message;
                }
            }
            return View(resetPassword);
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
