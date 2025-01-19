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
                // set by default ApproveStatus Pending
                if(question.ApproveStatus == null)
                {
                    question.ApproveStatus = "Pending";
                }
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

                    questions = await _context.Questions.Where(q =>(q.AddedBy == null) && (q.CourseId == courseId || courseId == null) && (q.TopicId == topicId || topicId == null) && (q.SubTopicId == subTopicId || subTopicId == null) && (!onlyActiveQuestions || q.IsActive)).ToListAsync();
                    return new ResponseModel { StatusCode = 200, Data = questions, Message = "Questions retrieved successfully" };

                }
                else
                {
                    // use pagination
                    questions = await _context.Questions.Where(q => (q.CourseId == courseId || courseId == null) && (q.TopicId == topicId || topicId == null) && (q.SubTopicId == subTopicId || subTopicId == null) && (!onlyActiveQuestions || q.IsActive)).Skip((pageNumber-1)*pageSize??1).Take(pageSize??5).ToListAsync();
                    totalQuestions = await _context.Questions.Where(q => (q.CourseId == courseId || courseId == null) && (q.TopicId == topicId || topicId == null) && (q.SubTopicId == subTopicId || subTopicId == null) && (!onlyActiveQuestions || q.IsActive)).CountAsync();
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
                else
                {
                    //_context.Entry(questionExists).CurrentValues.SetValues(question);
                    questionExists.Question = question.Question;
                    questionExists.QuestionAnswer = question.QuestionAnswer;
                    questionExists.IsActive = question.IsActive;
                    questionExists.TechStack = question.TechStack;
                    questionExists.AddedBy = question.AddedBy;
                    questionExists.CompanyName = question.CompanyName;
                    questionExists.ApproveStatus = question.ApproveStatus;
                    questionExists.CourseId = question.CourseId;
                    questionExists.TopicId = question.TopicId;
                    questionExists.SubTopicId = question.SubTopicId;
                    questionExists.DifficultyLevelId = question.DifficultyLevelId;

                    await _context.SaveChangesAsync();
                    return new ResponseModel { StatusCode = 200, Message = "Question updated successfully" };
                }
                
                
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> GetInterviewQuestions(Guid? addeddById, int? pageNumber, int? pageSize,string? companyName , string? techStack, bool onlyActiveQuestions, bool withAddedByDetails, bool onlyAcceptApprovalStatus)
        {

            try{
                List<QuestionModel> questions = new List<QuestionModel>();
                int totalQuestions = 0;
                
                if(pageNumber == null){
                    if(withAddedByDetails){
                        questions = await _context.Questions.Include(q => q.AddedByAdminUser).Where(q => (!onlyAcceptApprovalStatus || q.ApproveStatus == "Accept") && (!onlyActiveQuestions || q.IsActive) && q.AddedBy != null && (addeddById == null || q.AddedBy == addeddById) &&  (string.IsNullOrEmpty(companyName) || q.CompanyName == companyName) &&  (string.IsNullOrEmpty(techStack) || q.TechStack == techStack)).ToListAsync();
                    }else{
                        questions = await _context.Questions.Where(q =>(!onlyAcceptApprovalStatus || q.ApproveStatus == "Accept") && (!onlyActiveQuestions || q.IsActive) && q.AddedBy != null && (addeddById == null || q.AddedBy == addeddById) &&  (string.IsNullOrEmpty(companyName) || q.CompanyName == companyName) &&  (string.IsNullOrEmpty(techStack) || q.TechStack == techStack)).ToListAsync();
                    }
                    return new ResponseModel { StatusCode = 200, Data = questions, Message = "Questions retrieved successfully" };
                }else{
                    // count total questions
                        totalQuestions =  await _context.Questions.Where(q =>(!onlyAcceptApprovalStatus || q.ApproveStatus == "Accept") && (!onlyActiveQuestions || q.IsActive) &&  q.AddedBy != null && (addeddById == null || q.AddedBy == addeddById) && (string.IsNullOrEmpty(companyName) || q.CompanyName == companyName) &&  (string.IsNullOrEmpty(techStack) || q.TechStack == techStack)).CountAsync();
               
                     if(withAddedByDetails){
                     questions = await _context.Questions.Include(q => q.AddedByAdminUser).Where(q => (!onlyAcceptApprovalStatus || q.ApproveStatus == "Accept") &&(!onlyActiveQuestions || q.IsActive) && q.AddedBy != null && (addeddById == null || q.AddedBy == addeddById) &&  (string.IsNullOrEmpty(companyName) || q.CompanyName == companyName) &&  (string.IsNullOrEmpty(techStack) || q.TechStack == techStack)).Skip((pageNumber-1)*pageSize??1).Take(pageSize??10).ToListAsync();
                    }else{
                        questions = await _context.Questions.Where(q =>(!onlyAcceptApprovalStatus || q.ApproveStatus == "Accept") && (!onlyActiveQuestions || q.IsActive) && q.AddedBy != null && (addeddById == null || q.AddedBy == addeddById) &&  (string.IsNullOrEmpty(companyName) || q.CompanyName == companyName) &&  (string.IsNullOrEmpty(techStack) || q.TechStack == techStack)).Skip((pageNumber-1)*pageSize??1).Take(pageSize??10).ToListAsync();
                    }
                }
                return new ResponseModel { StatusCode = 200, Data = new {totalQuestions,questions}, Message = "Questions retrieved successfully" };
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

        public async Task<ResponseModel> GetAllUniqueCompanyNamesAndTechStack()
        {
            try{
                List<string>? companyNames = await _context.Questions.Select(q =>q.CompanyName).Distinct().ToListAsync();
                List<string>? techStacks = await _context.Questions.Select(q => q.TechStack).Distinct().ToListAsync();
                return new ResponseModel { StatusCode = 200, Data = new{companyNames,techStacks}, Message = "Company names and TechStack retrieved successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}
