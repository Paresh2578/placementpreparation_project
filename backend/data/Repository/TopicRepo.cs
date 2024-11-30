using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class TopicRepo : TopicInterface
    {
        private readonly ApplicationDbContext _context;
        public TopicRepo(ApplicationDbContext context)
        {
            _context = context;
        }
       public async Task<ResponseModel> AddTopic(TopicModel topic)
        {
            try{
                await  _context.Topics.AddAsync(topic);
                await _context.SaveChangesAsync();
                return new ResponseModel{StatusCode = 201,Message = "Topic Added Successfully"};
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

       public async Task<ResponseModel> GetAllTopics()
        {
            try{
                // var topics = await _context.Topics.ToListAsync();
                var topics = await _context.Topics.Include(c => c.Course).Include(d => d.DifficultyLevel).ToListAsync();
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
    }
}
