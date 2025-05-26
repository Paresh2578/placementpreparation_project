
using backend.Models;

namespace backend.data.Interface
{ 
 public interface QuestionAnswerInterface
 {

    public Task<ResponseModel> GetAllAnswerByQuestionId(Guid questionId);
    public Task<ResponseModel> PostAnswer(QuestionAnswerModel answer);
    public Task<ResponseModel> DeleteAnswer(Guid answerId);
 }   
}