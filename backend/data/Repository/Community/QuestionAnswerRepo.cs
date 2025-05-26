using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{ 
    public class QuestionAnswerRepo : QuestionAnswerInterface
    {
        private readonly ApplicationDbContext _context;
        public QuestionAnswerRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> GetAllAnswerByQuestionId(Guid questionId)
        {
            try
            {
                List<QuestionAnswerModel> answers = await _context.QuestionAnswers
                .Where(a => a.QuestionId == questionId).ToListAsync();

            return new ResponseModel
            {
                Data = answers,
                Message = answers.Count > 0 ? "Answers retrieved successfully." : "No answers found for this question.",
                StatusCode = 200,
            };
            }
            catch
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving answers."
                };
            }
        }

        public async Task<ResponseModel> PostAnswer(QuestionAnswerModel answer)
        {
            try
            {
                _context.QuestionAnswers.Add(answer);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    Message = "Answer posted successfully.",
                    StatusCode = 201
                };
            }
            catch
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An error occurred while posting the answer."
                };
            }
        }

        public async Task<ResponseModel> DeleteAnswer(Guid answerId)
        {
            try
            {
                var answer = await _context.QuestionAnswers.FindAsync(answerId);
                if (answer == null)
                {
                    return new ResponseModel
                    {
                        StatusCode = 404,
                        Message = "Answer not found."
                    };
                }

                _context.QuestionAnswers.Remove(answer);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    Message = "Answer deleted successfully.",
                    StatusCode = 200
                };
            }
            catch
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the answer."
                };
            }
        }
    }
}