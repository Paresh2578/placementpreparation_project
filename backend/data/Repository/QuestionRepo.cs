using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.data.Repository
{
    public class QuestionRepo : QuestionInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly DbHelper _dbHelper;
        public QuestionRepo(ApplicationDbContext context, DbHelper dbHelper)
        {
            _context = context;
            _dbHelper = dbHelper;
        }
       public async Task<ResponseModel> AddQuestion(QuestionModel question)
        {
            try{
                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 201, Message = "Question added successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async  Task<ResponseModel> DeleteQuestion(QuestionModel question)
        {
            try{
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Question deleted successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> DeleteMultipleQuestion(List<Guid> questionIds){
        try{
           // get all question to deleted
            var questionsToDelete = await _context.Questions.Where(question => questionIds.Contains(question.QuestionId)).ToListAsync();

            _context.Questions.RemoveRange(questionsToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Questions"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }

       public async Task<ResponseModel> GetAllQuestions(Guid? courseId, Guid? topicId, Guid? subTopicId,int? pageNumber , int? pageSize , bool onlyActiveQuestions)
        {
            try{
                List<QuestionModel> questions = new List<QuestionModel>();
                int totalQuestions = 0;

                //check pagination apply or not
                if (pageNumber == null)
                {
                    // get all questions
                    questions = await _context.Questions.Where(q => (q.CourseId == courseId || courseId == null) && (q.TopicId == topicId || topicId == null) && (q.SubTopicId == subTopicId || subTopicId == null) && (onlyActiveQuestions ? q.IsActive : true)).ToListAsync();
                    return new ResponseModel { StatusCode = 200, Data = questions, Message = "Questions retrieved successfully" };

                }
                else
                {
                    // use pagination
                    questions = await _context.Questions.Where(q => (q.CourseId == courseId || courseId == null) && (q.TopicId == topicId || topicId == null) && (q.SubTopicId == subTopicId || subTopicId == null) && (onlyActiveQuestions ? q.IsActive : true)).Skip((pageNumber-1)*pageSize??1).Take(pageSize??5).ToListAsync();

                    // count total 
                    using(SqlCommand command = _dbHelper.getSqlCommand("PR_Question_COUNT"))
                    {
                        command.Parameters.Add("@onlyActiveGets", SqlDbType.Bit).Value = onlyActiveQuestions ? 1 : 0;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                totalQuestions = reader.GetInt32(0);
                            }
                        }
                    }
                }
                return new ResponseModel { StatusCode = 200, Data = new {totalQuestions,questions} , Message = "Questions retrieved successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async  Task<ResponseModel> GetQuestionById(Guid id)
        {
            try{
                QuestionModel? question = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == id);
                if(question is null)
                {
                    return new ResponseModel { StatusCode = 404, Message = "Question not found" };
                }
                return new ResponseModel { StatusCode = 200, Data = question , Message = "Question retrieved successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public  async Task<ResponseModel> UpdateQuestion(QuestionModel question)
        {
            try{
                // Validate if the question exists
                QuestionModel? questionExists = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == question.QuestionId);
                if(questionExists is null)
                {
                    return new ResponseModel { StatusCode = 404, Message = "Question not found" };
                }
                 _context.Entry(questionExists).CurrentValues.SetValues(question);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Question updated successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> GetInterviewQuestions(Guid? addeddById)
        {
            try{
                List<QuestionModel> questions = await _context.Questions.Where(q => q.AddedBy != null && (addeddById == null || q.AddedBy == addeddById)).ToListAsync();
                return new ResponseModel { StatusCode = 200, Data = questions, Message = "Questions retrieved successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> UpdateNewInterviewQuestionRequestStatus(Guid id, string status)
        {
            try{
                await _context.Database.ExecuteSqlRawAsync("UPDATE Questions SET ApproveStatus = {0} Where QuestionId = {1}", status ,id);
                return new ResponseModel { StatusCode = 200, Message = "Question status updated successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}
