using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class FeedbackRepo : FeedbackInterface
    { 
        private readonly ApplicationDbContext _context;

        public FeedbackRepo(ApplicationDbContext context){
            _context = context;
        }
        public async Task<ResponseModel> AddFeedback(FeedbackModel feedback)
        {
            try{
                await _context.AddAsync(feedback);
               await _context.SaveChangesAsync();

               return new ResponseModel{StatusCode=201, Message="Feedback Added Successfully"};
            }catch{
                return new ResponseModel{
                    StatusCode = 500,
                    Message = "Internal Server Error",
                };
            }
        }

        public Task<ResponseModel> DeleteFeedback(FeedbackModel feedback)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> GetAllFeedback()
        {
            try{
                var feedbacks = await _context.Feedbacks.ToListAsync();
                return new ResponseModel{
                    StatusCode = 200,
                    Message = "Successfully retrieved feedbacks",
                    Data = feedbacks
                };
            }catch{
                return new ResponseModel{
                    StatusCode = 500,
                    Message = "Internal Server Error",
                };
            }
        }
    }
}