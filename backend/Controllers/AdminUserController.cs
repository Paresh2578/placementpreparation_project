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

        [HttpPost("signup")]
        public async  Task<IActionResult> SignUp([FromBody] AdminUserModel adminUser )
        {

            // check if the email exists
            var emailCheck = await _adminUserRepo.CheckEmailExist(adminUser.Email);
                if (!emailCheck.Success)
                {
                    return Conflict(emailCheck);
                }
            ResponseModel response = await _adminUserRepo.AddAdminUser(adminUser);

            if(!response.Success) {
                return BadRequest(response);
            }

             return CreatedAtAction(nameof(SignUp), new { id = adminUser.AdminUserId }, new
                {
                    message = "User signed up successfully",
                    success = true,
                });
        }
   
       [HttpPost("signin/{email}/{password}")]
        public async Task<IActionResult> SignIn(String email,String password)
        {
            // validate the email and password
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)){
                return BadRequest(new ResponseModel { Success = false, Message = "Email and Password are required." });
            }

             // check if the email exists
            ResponseModel response = await _adminUserRepo.SignIn(email, password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

                // Generate token
                var token = TokenGenerator.CreateToken(response.Data!.AdminUserId.ToString());

                // Set cookie with token
                    _httpContextAccessor.HttpContext!.Response.Cookies.Append("token", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true, // Set to true in production
                        SameSite = SameSiteMode.None, // Adjust as needed
                        Expires = DateTime.Now.AddDays(2)
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

        [HttpDelete("delete/{adminUserId}")]  
        public async Task<IActionResult> DeleteAdminUser(Guid adminUserId)
        {
            // find the admin user
            ResponseModel adminUser = await _adminUserRepo.FindAdminUsersById(adminUserId);
            if(!adminUser.Success) {
                return NotFound(adminUser);
            }

            ResponseModel response = await _adminUserRepo.DeleteAdminUser(adminUser.Data);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
   
       //[HttpPut]
       // public async Task<IActionResult> UpdateAdminUser([FromBody] AdminUserModel adminUser)
       // {
       //     // verify admin user id
       //     ResponseModel adminUserResponse = await _adminUserRepo.FindAdminUsersById(adminUser.AdminUserId);
       //     if(!adminUserResponse.Success) {
       //         return NotFound(adminUserResponse);
       //     }
            
       //     adminUser.Password = adminUserResponse.Data!.Password;
       //     ResponseModel response = await _adminUserRepo.UpdateAdminUser(adminUser);
       //     if (!response.Success)
       //     {
       //         return BadRequest(response);
       //     }

       //     return Ok(response);
       // }
    }
}
