using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.data.Repository;
using backend.Constant;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly AdminUserRepo _adminUserRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminUserController(AdminUserRepo adminUserRepo , IHttpContextAccessor httpContextAccessor)
        {
            _adminUserRepo = adminUserRepo;
            _httpContextAccessor = httpContextAccessor;
        }


        #region  Signup Admin User
        [CheckAccess]
        [HttpPost("signup")]
        public async  Task<IActionResult> SignUp([FromBody] AdminUserModel adminUser )
        {

            // check if the email alredy exists or not
            var emailCheck = await _adminUserRepo.CheckEmailExist(adminUser.Email);
            if(emailCheck.StatusCode != 200) {
                return StatusCode(emailCheck.StatusCode, emailCheck);
            }

            // add the admin user
            ResponseModel response = await _adminUserRepo.AddAdminUser(adminUser);

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

             // check if the email exists
            ResponseModel response = await _adminUserRepo.SignIn(email, password);
             if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode,response);
            }


                // Generate token
                var token = TokenGenerator.CreateToken(response.Data!.AdminUserId.ToString());


                // Set cookie with token
                    _httpContextAccessor.HttpContext!.Response.Cookies.Append("token", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true, // Set to true in production
                        SameSite = SameSiteMode.None, // Adjust as needed
                        // Expires = DateTime.Now.AddDays(2)
                        Expires = DateTime.Now.AddHours(1)
                    });

                // set cookie with  user data 
                var cookieData = new
                {
                    response.Data!.UserName,
                    response.Data!.Email
                };
                _httpContextAccessor.HttpContext!.Response.Cookies.Append("userData", Newtonsoft.Json.JsonConvert.SerializeObject(cookieData), new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true, // Set to true in production
                    SameSite = SameSiteMode.None, // Adjust as needed
                    Expires = DateTime.Now.AddDays(2)
                }); 


            return Ok(new { message = "User signed in successfully", success = true });
        }
        #endregion
   
        #region Delete Admin
        [CheckAccess]
        [HttpDelete("delete/{adminUserId}")]  
        public async Task<IActionResult> DeleteAdminUser(Guid adminUserId)
        {
            // find the admin user
            ResponseModel adminUser = await _adminUserRepo.FindAdminUsersById(adminUserId);
            if(adminUser.StatusCode != 200) {
                return StatusCode(adminUser.StatusCode, adminUser);
            }

            ResponseModel response = await _adminUserRepo.DeleteAdminUser(adminUser.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
