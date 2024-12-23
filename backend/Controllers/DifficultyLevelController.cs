using backend.Constant;
using backend.data.Repository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyLevelController : ControllerBase
    {
        private readonly DifficultyLevelRepo _difficultyLevelRepo;
        public DifficultyLevelController(DifficultyLevelRepo difficultyLevelRepo)
        {
            _difficultyLevelRepo = difficultyLevelRepo;
        }

        #region Get All Difficulty Levels
        [HttpGet]
        public async Task<IActionResult> GetAllDifficultyLevels()
        {
            var response = await _difficultyLevelRepo.GetAllDifficultyLevels();
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Get Difficulty Level By Id
        [HttpGet("{difficultyLevelId}")]
        public async Task<IActionResult> GetDifficultyLevelById(Guid difficultyLevelId)
        {
            var response = await _difficultyLevelRepo.GetDifficultyLevelById(difficultyLevelId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
        
        #region Add Difficulty Level
        [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddDifficultyLevel([FromBody] DifficultyLevelModel difficultyLevelModel)
        {
            var response = await _difficultyLevelRepo.AddDifficultyLevel(difficultyLevelModel);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
        
        #region Update Difficulty Level
        [CheckAccess]
        [HttpPut()]
        public async Task<IActionResult> UpdateDifficultyLevel([FromBody] DifficultyLevelModel difficultyLevelModel)
        {
            var response = await _difficultyLevelRepo.UpdateDifficultyLevel(difficultyLevelModel);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
   
        #region Delete Difficulty Level
        [CheckAccess]
        [HttpDelete("{difficultyLevelId}")]
        public async Task<IActionResult> DeleteDifficultyLevel(Guid difficultyLevelId)
        {
            // Check if the difficulty level exists
            var difficultyLevel = await _difficultyLevelRepo.GetDifficultyLevelById(difficultyLevelId);
            if(difficultyLevel.StatusCode != 200){
                return StatusCode(difficultyLevel.StatusCode, difficultyLevel);
            }

            var response = await _difficultyLevelRepo.DeleteDifficultyLevel(difficultyLevelId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

         #region Delete Multiple Difficulty Level
        [HttpDelete("DeleteMultiple")]
        [CheckAccess]
        public async Task<IActionResult> DeleteMultipleDifficultyLevel([FromBody] List<Guid> difficultyLevelIds){
            var response = await _difficultyLevelRepo.DeleteMultipleDifficultyLevel(difficultyLevelIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion
    }
}
