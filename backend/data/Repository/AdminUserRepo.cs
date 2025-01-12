using backend.Constant;
using backend.data.Interface;
using Microsoft.AspNetCore.Http;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using backend.Service;

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
                ResponseModel response = await _emailService.SendEmail(email, "Email Varification", $"Your OTP is {otp}");
                if(response.StatusCode != 200){
                    return response;
                }

                // Store OTP in session for verification (for simplicity in this example)
             _httpContextAccessor.HttpContext.Session.SetString($"{email}OTP", otp.ToString());
                
                return new ResponseModel { StatusCode= 200, Message = "OTP sent successfully." };
            }
            catch {
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

        public async Task<ResponseModel> VarifyOtp(string email, string otp)
        {
            try
            {
                // Get the OTP from session
                var sessionOtp = _httpContextAccessor.HttpContext.Session.GetString($"{email}OTP");
                if(sessionOtp is null){
                    return new ResponseModel { StatusCode= 400, Message = "OTP expired." };
                }

                // Verify the OTP
                if(sessionOtp != otp){
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
    }
}
