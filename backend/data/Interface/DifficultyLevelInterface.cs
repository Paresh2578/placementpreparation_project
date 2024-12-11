using backend.Models;

namespace backend.data.Interface
{
    public interface DifficultyLevelInterface
    {
        public Task<ResponseModel> AddDifficultyLevel(DifficultyLevelModel difficultyLevelModel);
        public Task<ResponseModel> GetAllDifficultyLevels();
        public Task<ResponseModel> GetDifficultyLevelById(Guid difficultyLevelId);
        public Task<ResponseModel> UpdateDifficultyLevel(DifficultyLevelModel difficultyLevelModel);
        public Task<ResponseModel> DeleteDifficultyLevel(Guid difficultyLevelId);

        public Task<ResponseModel> DeleteMultipleDifficultyLevel(List<Guid> difficultyLevelIds);
    }
}
