using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class DifficultyLevelRepo : DifficultyLevelInterface
    {
        private readonly ApplicationDbContext _context;
        public DifficultyLevelRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel> AddDifficultyLevel(DifficultyLevelModel difficultyLevelModel)
        {
            try{    
                // Add the difficulty level to the database
                await _context.DifficultyLevels.AddAsync(difficultyLevelModel);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 201, Message = "Difficulty level added successfully." };
            }catch{
                return new ResponseModel { StatusCode = 500, Message = "An error occurred while adding the difficulty level." };
            }
        }

        public  async Task<ResponseModel> DeleteDifficultyLevel(Guid difficultyLevelId)
        {
            try{
                // Find the difficulty level by id
                var difficultyLevel = await _context.DifficultyLevels.FindAsync(difficultyLevelId);
                if(difficultyLevel == null){
                    return new ResponseModel { StatusCode = 404, Message = "Difficulty level not found." };
                }
                // Remove the difficulty level from the database
                _context.DifficultyLevels.Remove(difficultyLevel);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Difficulty level deleted successfully." };
            }catch{
                return new ResponseModel { StatusCode = 500, Message = "An error occurred while deleting the difficulty level." };
            }
        }
       public async Task<ResponseModel> DeleteMultipleDifficultyLevel(List<Guid> difficultyLevelIds){
        try{
           // get all difficulty to deleted
            var difficultyLevelToDelete = await _context.DifficultyLevels.Where(difficultyLevel => difficultyLevelIds.Contains(difficultyLevel.DifficultyLevelId)).ToListAsync();

            _context.DifficultyLevels.RemoveRange(difficultyLevelToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Difficulty"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }

        public async Task<ResponseModel> GetAllDifficultyLevels()
        {
            try{
                // Get all difficulty levels from the database
                var difficultyLevels = await _context.DifficultyLevels.ToListAsync();
                return new ResponseModel { StatusCode = 200, Data = difficultyLevels , Message = "Difficulty levels retrieved successfully." };
            }catch{
                return new ResponseModel { StatusCode = 500, Message = "An error occurred while retrieving difficulty levels." };
            }
        }

       public async Task<ResponseModel> GetDifficultyLevelById(Guid difficultyLevelId)
        {
            try{
                // Find the difficulty level by id
                var difficultyLevel = await _context.DifficultyLevels.FindAsync(difficultyLevelId);
                if(difficultyLevel == null){
                    return new ResponseModel { StatusCode = 404, Message = "Difficulty level not found." };
                }
                return new ResponseModel { StatusCode = 200, Data = difficultyLevel , Message = "Difficulty level retrieved successfully." };
            }catch{
                return new ResponseModel { StatusCode = 500, Message = "An error occurred while retrieving the difficulty level." };
            }
        }

       public async Task<ResponseModel> UpdateDifficultyLevel(DifficultyLevelModel difficultyLevelModel)
        {
            try{
                // Find the difficulty level by id
                var difficultyLevel = await _context.DifficultyLevels.FindAsync(difficultyLevelModel.DifficultyLevelId);
                if(difficultyLevel == null){
                    return new ResponseModel { StatusCode = 404, Message = "Difficulty level not found." };
                }
                // Update the difficulty level in the database
                difficultyLevel.DifficultyLevelName = difficultyLevelModel.DifficultyLevelName;
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Difficulty level updated successfully." };
            }catch{
                return new ResponseModel { StatusCode = 500, Message = "An error occurred while updating the difficulty level." };
            }
        }
    }
}
