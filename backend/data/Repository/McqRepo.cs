using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class McqRepo : McqInterface
    {
        private readonly ApplicationDbContext _context;
        public McqRepo(ApplicationDbContext context)
        {
            _context = context;
        }
       public async Task<ResponseModel> AddMcq(McqModel mcq)
        {
            try{
                 await _context.AddAsync(mcq);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 201, Message = "Mcq Added Successfully" };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> DeleteMcq(McqModel mcq)
        {
            try{
                _context.Remove(mcq);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Mcq Deleted Successfully" };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

       public async Task<ResponseModel> GetAllMcq()
        {
            try{
                var mcqs = await _context.Mcqs.ToListAsync();
                return new ResponseModel { StatusCode = 200, Data = mcqs  , Message = "Mcq Fetched Successfully" };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
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
    }
}
