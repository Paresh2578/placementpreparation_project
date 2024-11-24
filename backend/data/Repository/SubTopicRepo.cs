using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class SubTopicRepo : SubTopicInterface
    {
        private readonly ApplicationDbContext _context;
        public SubTopicRepo(ApplicationDbContext context)
        {
            _context = context;
        }
       public async Task<ResponseModel> CreateSubTopic(SubTopicModel subTopic)
        {
            try{
                await _context.SubTopics.AddAsync(subTopic);
                await _context.SaveChangesAsync();

                return new ResponseModel{ StatusCode = 201, Message = "SubTopic Created Successfully" };
            }catch(Exception ex){
                return new ResponseModel{ StatusCode = 500, Message = ex.Message };
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

       public async Task<ResponseModel> GetAllSubTopics()
        {
            try{
                var subTopics = await _context.SubTopics.Include(t => t.Topic).ToListAsync();
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
    }
}
