using backend.Models;

namespace backend.data.Interface
{
    public interface QuestionInterface
    {
      public Task<ResponseModel> GetAllQuestions(Guid? courseId , Guid? topicId , Guid? subTopicId ,int? pageNumber , int? pageSize, bool onlyActiveQuestions);
      public Task<ResponseModel> GetQuestionById(Guid id);
      public Task<ResponseModel> AddQuestion(QuestionModel question);
      public Task<ResponseModel> UpdateQuestion(QuestionModel question);
      public Task<ResponseModel> DeleteQuestion(QuestionModel question);  

        public Task<ResponseModel> DeleteMultipleQuestion(List<Guid> questionIds); 
    }
}
