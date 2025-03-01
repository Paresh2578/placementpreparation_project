using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace backend.data.Repository
{
    public class TopicRepo : TopicInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly DbHelper _dbHelper;
        public TopicRepo(ApplicationDbContext context , DbHelper dbHelper)
        {
            _context = context;
            _dbHelper = dbHelper;
        }
       public async Task<ResponseModel> AddTopic(TopicModel topic)
        {
            try{
               // await  _context.Topics.AddAsync(topic);
               // await _context.SaveChangesAsync();
               using(SqlCommand cmd = _dbHelper.getSqlCommand("PR_Topic_AddTopic"))
                {
                    cmd.Parameters.Add("@TopicId", SqlDbType.UniqueIdentifier).Value = topic.TopicId;
                    cmd.Parameters.Add("@TopicName", SqlDbType.VarChar).Value = topic.TopicName;
                    cmd.Parameters.Add("@Content", SqlDbType.VarChar).Value = topic.Content;
                    cmd.Parameters.Add("@CourseId", SqlDbType.UniqueIdentifier).Value = topic.CourseId;
                    cmd.Parameters.Add("@DifficultyLevelId", SqlDbType.UniqueIdentifier).Value = topic.DifficultyLevelId;

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if(rowsAffected < 0)
                    {
                        return new ResponseModel { StatusCode = 201, Message = "Topic Added Successfully" };
                    }
                }
                return new ResponseModel{StatusCode = 500,Message = "Topic insertion failed." };
            }catch(Exception ex){
                return new ResponseModel{StatusCode = 500,Message = ex.Message};
            }
        }

       public async Task<ResponseModel> DeleteTopic(TopicModel topic)
        {
            try{
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
                return new ResponseModel{StatusCode = 200,Message = "Topic Deleted Successfully"};
            }catch(Exception ex){
                return new ResponseModel{StatusCode = 500,Message = ex.Message};
            }
        }

        public async Task<ResponseModel> DeleteMultipleTopic(List<Guid> topicIds){
        try{
           // get all topic to deleted
            var topicsToDelete = await _context.Topics.Where(topic => topicIds.Contains(topic.TopicId)).ToListAsync();

            _context.Topics.RemoveRange(topicsToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Topics"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }
       public async Task<ResponseModel> GetAllTopics()
        {
            try{
                var topics = await _context.Topics.Include(c => c.Course).Include(d => d.DifficultyLevel).OrderBy(t => t.Level).ToListAsync();
                return new ResponseModel{StatusCode = 200,Message = "Successfully Get All Topics",Data = topics};
            }catch(Exception ex){
                return new ResponseModel{StatusCode = 500,Message = ex.Message};
            }
        }

       public async Task<ResponseModel> GetTopicById(Guid topicId)
        {
            try{
                TopicModel? topic = await _context.Topics.Include(c => c.Course).Include(d => d.DifficultyLevel).FirstOrDefaultAsync(x => x.TopicId == topicId);

                if(topic is null){
                    return new ResponseModel{StatusCode = 404,Message = "Topic Not Found"};
                }

                return new ResponseModel{StatusCode = 200,Message = "Successfully Get Topic",Data = topic};
            }catch(Exception ex){
                return new ResponseModel{StatusCode = 500,Message = ex.Message};
            }
        }

       public async Task<ResponseModel> UpdateTopic(TopicModel topic)
        {
            try{
                // Validate if the topic  exists or not
                TopicModel? topicExist = await _context.Topics.FirstOrDefaultAsync(x => x.TopicId == topic.TopicId);
                if(topicExist is null){
                    return new ResponseModel{StatusCode = 404,Message = "Topic Not Found"};
                }

                // Update the topic
                _context.Entry(topicExist).CurrentValues.SetValues(topic);
                await _context.SaveChangesAsync();

                return new ResponseModel{StatusCode = 200,Message = "Topic Updated Successfully"};
            }catch(Exception ex){
                return new ResponseModel{StatusCode = 500,Message = ex.Message};
            }
        }
   
       public async Task<ResponseModel> GetTopicsByCourseId(Guid courseId)
        {
            try{
                var topics = await _context.Topics.Where(x => x.CourseId == courseId).ToListAsync();
                return new ResponseModel{StatusCode = 200,Message = "Successfully Get All Topics",Data = topics};
            }catch(Exception ex){
                return new ResponseModel{StatusCode = 500,Message = ex.Message};
            }
        }

        public async Task<ResponseModel> TopicDropdown(Guid? courseId)
        {
            try{
                // if courseId is null, return all topics
                var topics = new object();
                if(courseId is null || courseId == Guid.Empty){
                     topics = await _context.Topics.Select(x => new {x.TopicId,x.TopicName}).ToListAsync();
                }else{
                     topics = await _context.Topics.Where(x => x.CourseId == courseId).Select(x => new {x.TopicId,x.TopicName}).ToListAsync();
                }
                return new ResponseModel{StatusCode = 200,Message = "Successfully Get All Topics",Data = topics};
            }catch(Exception ex){
                return new ResponseModel{StatusCode = 500,Message = ex.Message};
            }
        }
    
         public async Task<ResponseModel> GetTopicsLengthByCourseId(Guid courseId)
          {
                try{
                 var topics = await _context.Topics.Where(x => x.CourseId == courseId).ToListAsync();
                 return new ResponseModel{StatusCode = 200,Message = "Successfully Get Topic Length",Data =new {length = topics.Count}};
                }catch(Exception ex){
                 return new ResponseModel{StatusCode = 500,Message = ex.Message};
                }
          }

        public async Task<ResponseModel> GetSidebarDataByCourseIdAndTopicDocumentionByTopicId(Guid courseId,Guid topicId,Guid? subTopicId)
        {
            try
            {
                var dataTable = new DataTable();
                using(SqlCommand command = _dbHelper.getSqlCommand("PR_Topic_SubTopic_List_ByCourseId"))
                {
                    command.Parameters.Add("@courseId", SqlDbType.UniqueIdentifier).Value = courseId;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }

                // Transform the data for sidebar
                /**
                 * [
                 *   {
                 *     topicId : ,
                 *     topicName : , 
                 *     level : ,
                 *     subTopicList : [
                 *        {
                 *           subTopicId : , 
                 *           subTopicName : , 
                 *           level :
                 *        }
                 *     ]
                 *    }
                 *  ]  
                 */
                var topics = dataTable.AsEnumerable()
                    .GroupBy(row => new
                    {
                        TopicId = row["TopicId"].ToString(),
                        TopicName = row["TopicName"].ToString(),
                        Level = row["Level"].ToString()
                    })
                    .Select(g => new
                    {
                        topicId = g.Key.TopicId,
                        topicName = g.Key.TopicName,
                        level = g.Key.Level,
                        subTopicList = g
                            .Where(row => !string.IsNullOrEmpty(row["SubTopicId"].ToString()))
                            .Select(row => new
                            {
                                subTopicId = row["SubTopicId"].ToString(),
                                subTopicName = row["SubTopicName"].ToString(),
                                level = row["SubTopicLevel"].ToString()
                            }).ToList()
                    }).ToList();

                // Get documention by topic Id
                //first check subTopic
                dynamic documention = null;
               //// documention = await _context.SubTopics.Select(s => new { s.Content,s.Level,s.TopicId}).OrderBy(s=>s.Level).FirstOrDefaultAsync(x => x.TopicId == topicId);

                // If subTopicId Not then documention get by subtopic id else get topicId ,first
               documention = await _context.SubTopics.Select(s => new { s.Content, s.Level, s.TopicId, s.SubTopicId }).OrderBy(s => s.Level).FirstOrDefaultAsync(x => subTopicId != null ? x.SubTopicId == subTopicId : x.TopicId == topicId);


                // check documention is null then get by topic
                if (documention == null)
                {
                    documention = await _context.Topics.Select(s=> new {s.Content,s.Level,s.TopicId}).OrderBy(s => s.Level).FirstOrDefaultAsync(x => x.TopicId == topicId);
                }

                // if documention is null then set default value
                if(documention == null) 
                {
                    documention = "No Available";
                }

                /**
                 * {
                 *   sidebar : topics,
                 *   documention : documention
                 * }
                */

                Dictionary<string, dynamic> respone = new Dictionary<string, dynamic>();
                respone.Add("sidebar", topics);
                respone.Add("documention", documention);

                //var topics = await _context.Topics.Include(s=> s.Sub)
                return new ResponseModel { StatusCode = 200, Message = "Successfully Get TopicList" , Data=respone };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}
