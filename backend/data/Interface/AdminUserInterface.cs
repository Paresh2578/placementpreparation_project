using backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace backend.data.Interface
{
    public interface AdminUserInterface
    {
        public  Task<ResponseModel>  AddAdminUser(AdminUserModel adminUser);
        public Task<ResponseModel> UpdateAdminUser(AdminUserModel adminUser);
        public Task<ResponseModel> DeleteAdminUser(AdminUserModel adminUser);
        public Task<ResponseModel> SignIn(String email, String password);
        public Task<ResponseModel> FindAdminUsersById(Guid adminUserId);
        public Task<ResponseModel> CheckEmailExist(String email);
        public Task<ResponseModel> SendOtp(String email);
        public Task<ResponseModel> VerifyOtp(string email,string otp);
        public Task<ResponseModel> UpdateApprovelStudentStatus(Guid id , String status);
        public Task<ResponseModel> RefreshToken(Guid id);

    }

}
