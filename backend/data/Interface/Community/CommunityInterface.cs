using backend.Models;

namespace backend.data.Interface
{
   public interface CommunityInterface
    {
        public Task<ResponseModel> GetAllQuestions();
        public Task<ResponseModel> GetQuestionById(Guid questionId);
        public Task<ResponseModel> AskQuestion(PostModel question);
    }
}