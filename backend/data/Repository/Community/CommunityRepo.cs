using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    class CommunityRepo : CommunityInterface
    {
        private readonly ApplicationDbContext _context;
        public CommunityRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> GetAllQuestions()
        {
            try
            {
                List<PostModel> questions = await _context.Posts
                    .ToListAsync();

                return new ResponseModel
                {
                    StatusCode = 200,
                    Data = questions,
                    Message = "Questions retrieved successfully."
                };
            }
            catch
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = "Failed to retrieve questions"
                };
            }
        }

        public async Task<ResponseModel> GetQuestionById(Guid questionId)
        {
            try
            {
                PostModel? question = await _context.Posts.FirstOrDefaultAsync(q => q.QuestionId == questionId);

                return new ResponseModel
                {
                    StatusCode = 200,
                    Data = question,
                    Message = question != null ? "Question retrieved successfully." : "Question not found."
                };
            }
            catch (Exception e)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = "Failed to retrieve question"
                };
            }
        }

        public async Task<ResponseModel> AskQuestion(PostModel question)
        {
            try
            {
                _context.Posts.Add(question);
                await _context.SaveChangesAsync();
                return new ResponseModel
                {
                    StatusCode = 201,
                    Message = "Question asked successfully."
                };
            }
            catch (Exception e)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = "Failed to ask question"
                };
            }
        }
    }
}