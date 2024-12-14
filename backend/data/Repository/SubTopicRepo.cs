using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.data.Repository
{
    public class SubTopicRepo : SubTopicInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly DbHelper _dbHelper;
        public SubTopicRepo(ApplicationDbContext context , DbHelper dbHelper)
        {
            _context = context;
            _dbHelper = dbHelper;
        }
       public async Task<ResponseModel> CreateSubTopic(SubTopicModel subTopic)
        {
            try
            {
                using (SqlCommand cmd = _dbHelper.getSqlCommand("PR_SubTopic_AddSubTopic"))
                {
                    cmd.Parameters.Add("@SubTopicId", SqlDbType.UniqueIdentifier).Value = subTopic.SubTopicId;
                    cmd.Parameters.Add("@SubTopicName", SqlDbType.VarChar).Value = subTopic.SubTopicName;
                    cmd.Parameters.Add("@Content", SqlDbType.VarChar).Value = subTopic.Content;
                    cmd.Parameters.Add("@CourseId", SqlDbType.UniqueIdentifier).Value = subTopic.CourseId;
                    cmd.Parameters.Add("@TopicId", SqlDbType.UniqueIdentifier).Value = subTopic.TopicId;
                    cmd.Parameters.Add("@DifficultyLevelId", SqlDbType.UniqueIdentifier).Value = subTopic.DifficultyLevelId;

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected < 0)
                    {
                        return new ResponseModel { StatusCode = 201, Message = "Sub Topic Added Successfully" };
                    }
                }
                return new ResponseModel { StatusCode = 500, Message = "Sub Topic insertion failed." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> DeleteSubTopic(SubTopicModel subTopic)
        {
            try{
                 _context.SubTopics.Remove(subTopic);
                await _context.SaveChangesAsync();
                
                return new ResponseModel{ StatusCode = 200, Message = "SubTopic Deleted Successfully" };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
            }   
        }

        public async Task<ResponseModel> DeleteMultipleSubTopic(List<Guid> subTopicIds){
        try{
           // get all sub topic to deleted
            var subTopicsToDelete = await _context.SubTopics.Where(sub => subTopicIds.Contains(sub.SubTopicId)).ToListAsync();

            _context.SubTopics.RemoveRange(subTopicsToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Sub Topics"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }

       public async Task<ResponseModel> GetAllSubTopics()
        {
            try{
                var subTopics = await _context.SubTopics.Include(t => t.Topic).OrderBy(s => s.Level).ToListAsync();
                return new ResponseModel{ StatusCode = 200, Data = subTopics , Message = "SubTopics Fetched Successfully" };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> GetSubTopicById(Guid subTopicId)
        {
            try{
                var subTopic = await _context.SubTopics.Include(t => t.Topic).FirstOrDefaultAsync(x => x.SubTopicId == subTopicId);
                if(subTopic is null){
                    return new ResponseModel{ StatusCode = 404, Message = "SubTopic Not Found" };
                }
                return new ResponseModel{ StatusCode = 200, Data = subTopic };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> UpdateSubTopic(SubTopicModel subTopic)
        {
            try{
               // validate if the subtopic exists
                var subTopicExist = await _context.SubTopics.FirstOrDefaultAsync(x => x.SubTopicId == subTopic.SubTopicId);
                if(subTopicExist is null){
                    return new ResponseModel{ StatusCode = 404, Message = "SubTopic Not Found" };
                }

                _context.Entry(subTopicExist).CurrentValues.SetValues(subTopic);
                await _context.SaveChangesAsync();

                return new ResponseModel{ StatusCode = 200, Message = "SubTopic Updated Successfully" };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
            }
        }
   
      
      public async Task<ResponseModel> GetSubTopicsByTopicId(Guid topicId)
        {
            try{
                var subTopics = await _context.SubTopics.Where(x => x.TopicId == topicId).ToListAsync();
                return new ResponseModel{ StatusCode = 200, Data = subTopics , Message = "SubTopics Fetched Successfully" };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
            }
        }
      public async Task<ResponseModel> GetSubTopicsByCourseId(Guid courseId)
        {
            try{
                var subTopics = await _context.SubTopics.Where(x => x.CourseId == courseId).ToListAsync();
                return new ResponseModel{ StatusCode = 200, Data = subTopics , Message = "SubTopics Fetched Successfully" };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
            }
        }
   
    public async Task<ResponseModel> SubTopicDropdown(Guid? courseId, Guid? topicId)
        {
            try{
                var query = _context.SubTopics
                    .Where(x => 
                        (courseId == null || courseId == Guid.Empty || x.CourseId == courseId) &&
                        (topicId == null || topicId == Guid.Empty || x.TopicId == topicId))
                    .Select(x => new { SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName });

             var subTopics = await query.ToListAsync();
                return new ResponseModel{ StatusCode = 200, Data = subTopics , Message = "SubTopics Fetched Successfully" };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> GetSubTopicsLengthByTopicId(Guid topicId)
        {
            try
            {
                var subTopics = await _context.SubTopics.Where(x => x.TopicId == topicId).ToListAsync();
                return new ResponseModel { StatusCode = 200, Message = "Successfully Get Sub Topic Length", Data = new { length = subTopics.Count } };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}
