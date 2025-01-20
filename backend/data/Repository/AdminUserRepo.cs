using backend.Constant;
using backend.data.Interface;
using Microsoft.AspNetCore.Http;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using backend.Service;
using System.Collections;
using Newtonsoft.Json;

namespace backend.data.Repository
{
    public class AdminUserRepo : AdminUserInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailService _emailService;
        public AdminUserRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, EmailService emailService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        public async Task<ResponseModel> AddAdminUser(AdminUserModel adminUser)
        {
            try
            {
                // hash the password
                adminUser.Password = PasswordHasher.HashPassword(password : adminUser.Password);

                // add the admin user
                await _context.AdminUsers.AddAsync(adminUser);
                await _context.SaveChangesAsync();
                
                // If successful, return a success response
                return new ResponseModel { StatusCode= 201 , Message = "Sign Up successfully." };
            }
            catch
            {
                // Return a failure response
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error"};
            }
        }

        public async Task<ResponseModel> CheckEmailExist(string email)
        {
            try
            {
                // Check if the email exists
                var emailExist = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Email == email);
                if (emailExist != null)
                {
                    return new ResponseModel { StatusCode= 409, Message = "Email already exists." };
                }
                else
                {
                    return new ResponseModel { StatusCode= 200 };
                }
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

        public async Task<ResponseModel> DeleteAdminUser(AdminUserModel adminUser)
        {
            try
            {
                _context.AdminUsers.Remove(adminUser);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode= 200, Message = "Admin user deleted successfully." };
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

        public async Task<ResponseModel> SignIn(string email, string password)
        {

            // remove white spaces from email , password
            email = email.Trim();
            password = password.Trim();

            try
            {
                // Find the user by email
                AdminUserModel? user = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new ResponseModel { StatusCode= 400, Message = "Invalid email." };
                }

                // Verify the password
                if (!PasswordHasher.VerifyPassword(password, user.Password))
                {
                    return new ResponseModel { StatusCode= 400, Message = "Invalid password." };
                }

                // Find the user by email and password
                user = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Email == email && x.Password == user.Password);
                if(user == null){
                    return new ResponseModel { StatusCode= 400, Message = "Invalid email or password." };
                }
               
                return new ResponseModel { StatusCode= 200, Data = user , Message = "User signed in successfully." };
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

        public async Task<ResponseModel> UpdateAdminUser(AdminUserModel adminUser)
        {
            try
            {
                _context.AdminUsers.Update(adminUser);
                await _context.SaveChangesAsync();

                return new ResponseModel { StatusCode= 200, Message = "Admin user updated successfully." };
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

       public async Task<ResponseModel> FindAdminUsersById(Guid adminUserId)
        {
            try{
                AdminUserModel? adminUser = await _context.AdminUsers.FirstOrDefaultAsync(x => x.AdminUserId == adminUserId);
                if(adminUser is null){
                    return new ResponseModel { StatusCode= 404, Message = "Admin user not found." };
                }
                return new ResponseModel { StatusCode= 200, Data = adminUser };
            }catch{
                  return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
               }
        }
   
        public async Task<ResponseModel> SendOtp(string email)
        {
            try
            {
                // Generate OTP
                var otp = OTPGenerator.GenerateOTP();

                // send OTP in Mail
                string subject = "OTP Verification";
                string bodyContent =$@"<h2 style=""font-size: 20px; color: #333333;"">Verify Your Email Address</h2>
                        <p>Thank you for signing up with <b>Placement Preparation</b>. To complete your registration, please use the OTP below to verify your email address.</p>

                        <!-- OTP Code -->
                        <div style=""display: inline-block; margin: 20px 0; font-size: 28px; font-weight: bold; color: #4CAF50; letter-spacing: 4px;"">{otp}</div>

                        <p>Please note that this OTP is valid for only <strong>15 minutes</strong>.</p>
                        <p>If you didn’t request this, please ignore this email.</p>";

                ResponseModel response = await _emailService.SendEmail(email, subject, bodyContent);
                if(response.StatusCode != 200){
                    return response;
                }


                // Set expiration time to 15 minutes from now
                var expirationTime = DateTime.UtcNow.AddMinutes(15);

                // Combine OTP and expiration time into a single object
                var otpData = new
                {
                    OTP = otp.ToString(),
                    Expiration = expirationTime
                };

                // Serialize the object to a JSON string for storage in session
                var otpJson = JsonConvert.SerializeObject(otpData);

                // Store in session
                _httpContextAccessor.HttpContext.Session.SetString($"{email}OTP", otpJson);
                                
               return new ResponseModel { StatusCode= 200, Message = "OTP sent successfully." };
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

        public async Task<ResponseModel> VerifyOtp(string email, string otp)
        {
            try
            {
                // Retrieve the OTP data from session
                var otpJson = _httpContextAccessor.HttpContext.Session.GetString($"{email}OTP");

                if (string.IsNullOrEmpty(otpJson))
                {
                    return new ResponseModel { StatusCode= 400, Message = "OTP expired." };
                }

                // Deserialize the JSON string back into an object
                var otpData = JsonConvert.DeserializeObject<dynamic>(otpJson);

                if(otpData is null){
                    return new ResponseModel { StatusCode= 400, Message = "OTP expired." };
                }

                // Check if the OTP has expired
                DateTime? expiration = otpData.Expiration;
                if (DateTime.UtcNow > expiration)
                {
                   return new ResponseModel { StatusCode= 400, Message = "OTP expired." };
                }

                // Verify the OTP
                if( otpData.OTP != otp){
                    return new ResponseModel { StatusCode= 400, Message = "Invalid OTP." };
                }

                // Clear the OTP from session
                _httpContextAccessor.HttpContext.Session.Remove($"{email}OTP");

                return new ResponseModel { StatusCode= 200, Message = "OTP verified successfully." };
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }
    
        public async Task<ResponseModel> UpdateApprovelStudentStatus(Guid id , string status)
        {
            try
            {
                // update student status
                await _context.Database.ExecuteSqlRawAsync("UPDATE AdminUsers SET ApproveStatus = {0} Where AdminUserId = {1}", status ,id);
                return new ResponseModel { StatusCode= 200, Message = "Student status updated successfully." };
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }
     

     public async Task<ResponseModel> RefreshToken(Guid id){
        try{
            //update user data
             ResponseModel response = await FindAdminUsersById(id);
            if(response.StatusCode != 200) {
                return new ResponseModel { StatusCode = response.StatusCode, Message = response.Message };
            }

            AdminUserModel adminUser = (AdminUserModel)response.Data!;
            adminUser.Password = null;

            // Generate new token
            var newToken = TokenGenerator.CreateToken(JsonConvert.SerializeObject(adminUser));

            // Set the 'token' cookie
            var commonCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.None, // Adjust as needed
                //Expires = DateTime.Now.AddDays(1)
                Expires = DateTime.Now.AddHours(1)
            };

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("token", newToken, commonCookieOptions);

            var data = new Hashtable
            {
                { "token", newToken  },
                {"data" , adminUser }
            };

            return  new ResponseModel { Message = "Token refreshed successfully", StatusCode = 200,Data = data };
        }catch{
            return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
        }
     }
      
      public async Task<ResponseModel> ForgetPassword(string email){
        try{
            // Find the user by email
            AdminUserModel? user = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return new ResponseModel { StatusCode= 400, Message = "Invalid email." };
            }

            string token = TokenGenerator.CreateToken(user.AdminUserId.ToString(),expireTimeInHours:0.15);
            DateTime expriryTime = DateTime.Now.AddHours(0.15);

            // update token in database
            await _context.Database.ExecuteSqlRawAsync("UPDATE AdminUsers SET Token = {0}, TokenExpiryTime = {1} Where AdminUserId = {2}", token , expriryTime , user.AdminUserId);

            // send mail to reset password from link
            string subject = "Reset Your Password";
            string url = "http://localhost:5033/Authentication/Auth/ResetPassword?token=" + token;
            string bodyContent = $@"
             <div style=""padding: 20px;"">
                        <p style=""color: #333333; font-size: 16px; line-height: 1.5;"">
                            You recently requested to reset your password for your <strong>Placement Preparation</strong> account. 
                            Click the button below to reset it. This password reset is only valid for the next <strong>15 minutes</strong>.
                        </p>
                        <div style=""text-align: center; margin: 20px 0;"">
                            <a href=""{url}"" 
                            style=""display: inline-block; background-color: #4CAF50; color: #ffffff; text-decoration: none; 
                            padding: 10px 20px; border-radius: 5px; font-size: 16px;"">
                            Reset Your Password
                            </a>
                        </div>
                        <p style=""color: #666666; font-size: 14px; line-height: 1.5;"">
                            If you didn’t request this, you can safely ignore this email. 
                            If you have any questions, feel free to contact our support team.
                        </p>
                    </div>";



            ResponseModel response = await _emailService.SendEmail(email, subject, bodyContent);
            if(response.StatusCode != 200){
                return response;
            }

                
            return new ResponseModel { StatusCode= 200, Message = "Link sent successfully." };
        }catch{
            return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
      }
      }


      public async Task<ResponseModel> ResetPassword(string newPassword , string token){
        try{
            // Find the user by token
            AdminUserModel? user = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Token == token);
            if (user == null)
            {
                return new ResponseModel { StatusCode= 400, Message = "Invalid token." };
            }

            // check token expiry
            if(user.TokenExpiryTime < DateTime.Now){
                return new ResponseModel { StatusCode= 400, Message = "Token expired." };
            }

            // hash the password
            user.Password = PasswordHasher.HashPassword(password : newPassword);

            // update password in database
            await _context.Database.ExecuteSqlRawAsync("UPDATE AdminUsers SET Password = {0}, Token = {1}, TokenExpiryTime = {2} Where AdminUserId = {3}", user.Password , null , null , user.AdminUserId);

            return new ResponseModel { StatusCode= 200, Message = "Password reset successfully." };
        }catch{
            return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
        }
      }
   
    }
}
