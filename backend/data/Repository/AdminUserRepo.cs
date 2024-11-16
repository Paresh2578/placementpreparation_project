using backend.Constant;
using backend.data.Interface;
using Microsoft.AspNetCore.Http;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace backend.data.Repository
{
    public class AdminUserRepo : AdminUserInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminUserRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseModel> AddAdminUser(AdminUserModel adminUser)
        {
            try
            {
                // hash the password
                adminUser.Password = PasswordHasher.HashPassword(password : adminUser.Password);

                // add the admin user
                await _context.adminUsers.AddAsync(adminUser);
                await _context.SaveChangesAsync();
                
                // If successful, return a success response
                return new ResponseModel { Success = true };
            }
            catch (Exception e)
            {
                // Return a failure response
                return new ResponseModel { Success = false, Message = e.Message };
            }
        }

        public async Task<ResponseModel> CheckEmailExist(string email)
        {
            try
            {
                // Check if the email exists
                var emailExist = await _context.adminUsers.FirstOrDefaultAsync(x => x.Email == email);
                if (emailExist != null)
                {
                    return new ResponseModel { Success = false, Message = "Email already exists." };
                }
                else
                {
                    return new ResponseModel { Success = true };
                }
            }
            catch (Exception e)
            {
                return new ResponseModel { Success = false, Message = $"Error: {e.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteAdminUser(AdminUserModel adminUser)
        {
            try
            {
                _context.adminUsers.Remove(adminUser);
                await _context.SaveChangesAsync();
                return new ResponseModel { Success = true, Message = "Admin user deleted successfully." };
            }
            catch (Exception e)
            {
                return new ResponseModel { Success = false, Message = $"Error: {e.Message}" };
            }
        }

        public async Task<ResponseModel> SignIn(string email, string password)
        {
            try
            {
                // Find the user by email
                AdminUserModel? user = await _context.adminUsers.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new ResponseModel { Success = false, Message = "Invalid email." };
                }

                // Verify the password
                if (!PasswordHasher.VerifyPassword(password, user.Password))
                {
                    return new ResponseModel { Success = false, Message = "Invalid password." };
                }

                // Find the user by email and password
                user = await _context.adminUsers.FirstOrDefaultAsync(x => x.Email == email && x.Password == user.Password);
                if(user == null){
                    return new ResponseModel { Success = false, Message = "Invalid email or password." };
                }

               
               
                return new ResponseModel { Success = true, Data = user , Message = "User signed in successfully." };
            }
            catch (Exception e)
            {
                return new ResponseModel { Success = false, Message = $"Error: {e.Message}" };
            }
        }

        public async Task<ResponseModel> UpdateAdminUser(AdminUserModel adminUser)
        {
            try
            {
                _context.adminUsers.Update(adminUser);
                await _context.SaveChangesAsync();

                return new ResponseModel { Success = true, Message = "Admin user updated successfully." };
            }
            catch (Exception e)
            {
                return new ResponseModel { Success = false, Message = $"Error: {e.Message}" };
            }
        }

       public async Task<ResponseModel> FindAdminUsersById(Guid adminUserId)
        {
            try{
                AdminUserModel? adminUser = await _context.adminUsers.FirstOrDefaultAsync(x => x.AdminUserId == adminUserId);
                if(adminUser == null){
                    return new ResponseModel { Success = false, Message = "Admin user not found." };
                }
                return new ResponseModel { Success = true, Data = adminUser };
            }catch(Exception e){
                  return new ResponseModel { Success = false, Message = $"Error: {e.Message}" };
               }
        }
    }
}
