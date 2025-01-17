using backend.BAL;
using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace backend.data.Repository
{
    public class McqRepo : McqInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DbHelper _dbHelper;
        public McqRepo(ApplicationDbContext context , DbHelper dbHelper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dbHelper = dbHelper;
            _httpContextAccessor = httpContextAccessor;
        }
       public async Task<ResponseModel> AddMcq(McqModel mcq)
        {
            try{
                // If  Approve status is null set default Pending 
                if(mcq.ApproveStatus == null){
                    mcq.ApproveStatus = "Pending";
                }
                 await _context.Mcqs.AddAsync(mcq);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 201, Message = "Mcq Added Successfully" };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> DeleteMcq(McqModel mcq)
        {
            try{
                _context.Mcqs.Remove(mcq);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Mcq Deleted Successfully" };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> DeleteMultipleMcq(List<Guid> mcqIds){
        try{
           // get all mcq to deleted
            var mcqsToDelete = await _context.Mcqs.Where(mcq => mcqIds.Contains(mcq.McqId)).ToListAsync();

            _context.Mcqs.RemoveRange(mcqsToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Mcqs"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }

        //public async Task<ResponseModel> GetAllMcq(Guid? courseId, Guid? topicId, Guid? subTopicId)
        //{
        //    try
        //    {
        //        List<McqModel> mcqs = new List<McqModel>();

        //        using (SqlCommand command = _dbHelper.getSqlCommand("PR_Mcq_GetAll"))
        //        {
        //            command.Parameters.Add("@CourseId", SqlDbType.UniqueIdentifier).Value = (object?)courseId ?? DBNull.Value;
        //            command.Parameters.Add("@TopicId", SqlDbType.UniqueIdentifier).Value = (object?)topicId ?? DBNull.Value;
        //            command.Parameters.Add("@SubTopicId", SqlDbType.UniqueIdentifier).Value = (object?)subTopicId ?? DBNull.Value;

        //            using (SqlDataReader reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    mcqs.Add(new McqModel
        //                    {
        //                        McqId = reader.GetGuid(0),
        //                        QuestionText = reader.GetString(1),
        //                        OptionA = reader.GetString(2),
        //                        OptionB = reader.GetString(3),
        //                        OptionC = reader.GetString(4),
        //                        OptionD = reader.GetString(5),
        //                        CorrectAnswer = reader.GetString(6),
        //                        AnswerDescription = reader.GetString(7),
        //                        IsActive = reader.GetBoolean(8),
        //                        CourseId = reader.GetGuid(9),
        //                        TopicId = reader.GetGuid(10),
        //                        SubTopicId =await  reader.IsDBNullAsync(11) ? (Guid?)null : reader.GetGuid(11),
        //                        DifficultyLevelId = reader.GetGuid(12),
        //                    });
        //                }
        //            }
        //        }

        //        return new ResponseModel { StatusCode = 200, Data = mcqs, Message = "MCQs fetched successfully." };
        //    }
        //    catch
        //    {
        //        // Log the exception
        //        return new ResponseModel { StatusCode = 500, Message = "An error occurred while fetching MCQs." };
        //    }
        //}

        public async Task<ResponseModel> GetAllMcq(Guid? courseId, Guid? topicId, Guid? subTopicId , int? pageSize, int? pageNumber, bool onlyActiveMcqs )
        {
            try
            {
                List<McqModel> mcqs = new List<McqModel>();
                int totalMcqs = 0;

                //check pagination is use or not
                if(pageNumber == null)
                {
                    // Get All Mcqs
                    mcqs = await _context.Mcqs.Where(m =>(m.AddedBy == null) && (m.CourseId == courseId || courseId == null) && (m.TopicId == topicId || topicId == null) && (m.SubTopicId == subTopicId || subTopicId == null) && (onlyActiveMcqs ? m.IsActive : true)).ToListAsync();

                    return new ResponseModel { StatusCode = 200, Data = mcqs, Message = "MCQs fetched successfully." };
                }
                else
                {
                    // apply pagination
                    mcqs = await _context.Mcqs.Where(m => (m.CourseId == courseId || courseId == null) && (m.TopicId == topicId || topicId == null) && (m.SubTopicId == subTopicId || subTopicId == null) && (onlyActiveMcqs ? m.IsActive : true)).Skip((pageNumber - 1) * pageSize??1).Take(pageSize??1).ToListAsync();

                    // total count
                    using (SqlCommand command = _dbHelper.getSqlCommand("PR_Mcq_COUNT"))
                    {
                        command.Parameters.Add("@onlyActiveGets", SqlDbType.Bit).Value = onlyActiveMcqs ? 1 : 0;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                totalMcqs = reader.GetInt32(0);
                            }
                        }
                    }
                }
                return new ResponseModel { StatusCode = 200, Data = new { totalMcqs, mcqs }, Message = "MCQs fetched successfully." };
            }
            catch
            {
                // Log the exception
                return new ResponseModel { StatusCode = 500, Message = "An error occurred while fetching MCQs." };
            }
        }



        public async Task<ResponseModel> GetMcqById(Guid mcqId)
        {
            try{
                McqModel? mcq = await _context.Mcqs.FirstOrDefaultAsync(x => x.McqId == mcqId);
                if(mcq is null){
                    return new ResponseModel { StatusCode = 404, Message = "Mcq Not Found" };
                }
                return new ResponseModel { StatusCode = 200, Data = mcq  , Message = "Mcq Fetched Successfully" };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> UpdateMcq(McqModel mcq)
        {
            try{
               // validate if mcq exist
                McqModel? mcqExist = await _context.Mcqs.FirstOrDefaultAsync(x => x.McqId == mcq.McqId);
                if(mcqExist is null){
                    return new ResponseModel { StatusCode = 404, Message = "Mcq Not Found" };
                }

                // update mcq
                _context.Entry(mcqExist).CurrentValues.SetValues(mcq);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Mcq Updated Successfully" };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> GetMcqsByTopicOrSubTopicId(Guid topicId , Guid? subTopicId)
        {
            try
            {
                List<McqModel> mcqs = await _context.Mcqs.Where(m => (m.TopicId == topicId) && (m.SubTopicId == subTopicId || subTopicId == null) && m.IsActive).ToListAsync();
                return new ResponseModel { StatusCode = 200, Message = "Successfully Get Mcqs", Data = mcqs };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

        
        public async Task<ResponseModel> GetInterviewMcqs(int? pageNumber, int? pageSize, bool onlyActiveMcqs)
        {
            try{
                // get login user data from token
                Dictionary<string,dynamic>? userData = CV.GetUserDataFromToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                string? addeddById = null;
                if(userData != null && (userData["IsAdmin"] == null || userData["AdminUserId"] == null)){
                    return new ResponseModel { StatusCode = 401, Message = "Unauthorized" };
                }else{
                    if(userData != null && userData["IsAdmin"] == false){
                        addeddById = userData["AdminUserId"];
                    }
                }
                List<McqModel> mcqs = new List<McqModel>();
                int totalMcqs = 0;
                if(pageNumber == null){
                    // get all mcqs
                    mcqs = await _context.Mcqs.Where(q => (!onlyActiveMcqs || q.IsActive) && ( q.AddedBy != null && (addeddById == null || q.AddedBy.ToString() == addeddById))).ToListAsync();
                    return new ResponseModel { StatusCode = 200, Data = mcqs, Message = "Mcqs retrieved successfully" };
                }else{
                    // apply pagination
                    mcqs = await _context.Mcqs.Where(q => (!onlyActiveMcqs || q.IsActive) && ( q.AddedBy != null && (addeddById == null || q.AddedBy.ToString() == addeddById))).Skip((pageNumber - 1) * pageSize??1).Take(pageSize??1).ToListAsync();
                    // total count
                    totalMcqs = await _context.Mcqs.Where(q => (!onlyActiveMcqs || q.IsActive) && ( q.AddedBy != null && (addeddById == null || q.AddedBy.ToString() == addeddById))).CountAsync();
                }
                return new ResponseModel { StatusCode = 200, Data = new {totalMcqs, mcqs}, Message = "Mcqs retrieved successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

          public async Task<ResponseModel> UpdateNewInterviewMcqRequestStatus(Guid id, string status)
        {
            try{
                await _context.Database.ExecuteSqlRawAsync("UPDATE Mcqs SET ApproveStatus = {0} Where McqId = {1}", status ,id);
                return new ResponseModel { StatusCode = 200, Message = "Mcq status updated successfully" };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }


    }
}
