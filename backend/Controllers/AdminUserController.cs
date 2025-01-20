using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.data.Repository;
using backend.Constant;
using backend.BAL;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Collections;
using backend.data.Interface;
using CloudinaryDotNet.Actions;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly AdminUserInterface _adminUserInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminUserController(AdminUserInterface adminUserInterface , IHttpContextAccessor httpContextAccessor)
        {
            _adminUserInterface = adminUserInterface;
            _httpContextAccessor = httpContextAccessor;
        }


        #region  Signup Admin User
        //[CheckAccess]
        [HttpPost("signup")]
        public async  Task<IActionResult> SignUp([FromBody] AdminUserModel adminUser )
        {
            // check if the email alredy exists or not
            var emailCheck = await _adminUserInterface.CheckEmailExist(adminUser.Email);
            if(emailCheck.StatusCode != 200) {
                return StatusCode(emailCheck.StatusCode, emailCheck);
            }

            // add the admin user
            ResponseModel response = await _adminUserInterface.AddAdminUser(adminUser);

            return StatusCode(response.StatusCode, response);
        }
        #endregion
   
       #region  SignIn Admin User
       [HttpPost("signin/{email}/{password}")]
        public async Task<IActionResult> SignIn(String email,String password)
        {
            // validate the email and password
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)){
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "Email and Password are required." });
            }

            // email validation
            if (!CommonMethods.IsValidEmail(email)){
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "Email is not valid." });
            }

             // check if the email exists
            ResponseModel response = await _adminUserInterface.SignIn(email, password);
             if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode,response);
            }

               // password is null
               response.Data!.Password = null;

                // Generate token
                var token = TokenGenerator.CreateToken(JsonConvert.SerializeObject(response.Data));

                var commonCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Set to true in production
                    SameSite = SameSiteMode.None, // Adjust as needed
                    //Expires = DateTime.Now.AddDays(1)
                    Expires = DateTime.Now.AddHours(1)
                };

                // Set the 'token' cookie
                _httpContextAccessor.HttpContext!.Response.Cookies.Append("token", token, commonCookieOptions);

                var userData = new
                {
                    response.Data!.UserName,
                    response.Data!.Email,
                };

                // _httpContextAccessor.HttpContext!.Response.Cookies.Append("userData", Newtonsoft.Json.JsonConvert.SerializeObject(cookieData), commonCookieOptions);



                var data = new Hashtable
                {
                    { "token", token },
                    { "userData", userData }
                };


            return Ok(new ResponseModel{ Message = "User signed in successfully", StatusCode = 200 , Data = data});
        }
        #endregion
   
        #region Delete Admin
        [CheckAccess]
        [HttpDelete("delete/{adminUserId}")]  
        public async Task<IActionResult> DeleteAdminUser(Guid adminUserId)
        {
            // find the admin user
            ResponseModel adminUser = await _adminUserInterface.FindAdminUsersById(adminUserId);
            if(adminUser.StatusCode != 200) {
                return StatusCode(adminUser.StatusCode, adminUser);
            }

            ResponseModel response = await _adminUserInterface.DeleteAdminUser(adminUser.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
        
       #region send Otp to mail
        [HttpPost]
        [Route("sendOtp/{email}")]
        public async Task<IActionResult> SendOtp(string email)
        {

            // check if the email is empty
            if (string.IsNullOrEmpty(email)){
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "Email is required." });
            }

            // check if the email alredy exists or not
            var emailCheck = await _adminUserInterface.CheckEmailExist(email);
            if(emailCheck.StatusCode != 200) {
                return StatusCode(emailCheck.StatusCode, emailCheck);
            }

            ResponseModel response = await _adminUserInterface.SendOtp(email);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

       #region  VerifyOtp Otp
        [HttpPost]
        [Route("VerifyOtp/{email}/{otp}")]
        public async Task<IActionResult> VerifyOtp(string email, string otp)
        {
           ResponseModel response = await _adminUserInterface.VerifyOtp(email, otp);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

       #region Update Approvel Student Status
       [HttpPut]
       [MainAdminAccess]
       [Route("UpdateApprovelStudentStatus/{id}/{Status}")]
       public async Task<IActionResult> UpdateApprovelStudentStatus(Guid id , string Status)
       {
           ResponseModel response = await _adminUserInterface.UpdateApprovelStudentStatus(id , Status);
           return StatusCode(response.StatusCode, response);
       }
        #endregion

        #region  refres token
        [HttpPost]
        [Route("refreshToken/{id}")]
        public async Task<IActionResult> RefreshToken(Guid id)
        {
           ResponseModel response = await _adminUserInterface.RefreshToken(id);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Forget Password
        [HttpPost]
        [Route("forgetPassword/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            // check if the email is empty
            if (string.IsNullOrEmpty(email)){
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "Email is required." });
            }

            ResponseModel response = await _adminUserInterface.ForgetPassword(email);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Reset Password
        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] Dictionary<string,string> resetPassword)
        {
            // check if the email is empty
            if (!resetPassword.ContainsKey("NewPassword") || !resetPassword.ContainsKey("Token") || string.IsNullOrEmpty(resetPassword["NewPassword"]) || string.IsNullOrEmpty(resetPassword["Token"])){
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "password or token is required." });
            }

            ResponseModel response = await _adminUserInterface.ResetPassword(resetPassword["NewPassword"], resetPassword["Token"]);
            return StatusCode(response.StatusCode, response);
        }
      }
        #endregion
}
