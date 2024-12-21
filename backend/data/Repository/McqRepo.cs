using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.data.Repository
{
    public class McqRepo : McqInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly DbHelper _dbHelper;
        public McqRepo(ApplicationDbContext context , DbHelper dbHelper)
        {
            _context = context;
            _dbHelper = dbHelper;
        }
       public async Task<ResponseModel> AddMcq(McqModel mcq)
        {
            try{
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

        public async Task<ResponseModel> GetAllMcq(Guid? courseId, Guid? topicId, Guid? subTopicId , bool onlyActiveMcqs)
        {
            try
            {
                List<McqModel> mcqs = await _context.Mcqs.Where(m => (m.CourseId == courseId || courseId == null) && (m.TopicId == topicId || topicId == null) && (m.SubTopicId == subTopicId || subTopicId == null) && (onlyActiveMcqs ? m.IsActive : true)).ToListAsync();
                return new ResponseModel { StatusCode = 200, Data = mcqs, Message = "MCQs fetched successfully." };
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


    }
}
