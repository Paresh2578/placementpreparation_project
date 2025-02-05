using backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace backend.data.Interface
{
    public interface FeedbackInterface
    {
        public Task<ResponseModel> AddFeedback(FeedbackModel feedback);
        public Task<ResponseModel> DeleteFeedback(FeedbackModel feedback);
        public Task<ResponseModel> GetAllFeedback();
    }
}