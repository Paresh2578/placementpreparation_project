using Microsoft.AspNetCore.Mvc;
using backend.Constant;
using backend.Models;
using backend.data.Repository;
using backend.data;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly BranchRepo _branchRepo;

        public BranchController(BranchRepo branchRepo )
        {
            _branchRepo = branchRepo;
        }

        #region add Branch
        [CheckAccess]
        [HttpPost("")]
        public async Task<IActionResult> AddBranch([FromBody] BranchModel branch)
        {
            ResponseModel response = await _branchRepo.AddBranch(branch);
            return StatusCode(response.StatusCode, response);
        }

        #endregion

        #region delete Branch
        [CheckAccess]
        [HttpDelete("{branchId}")]
        public async Task<IActionResult> DeleteBranch(Guid branchId)
        {

            // check if branch exist
            ResponseModel response = await _branchRepo.GetBranchById(branchId);
            if (response.StatusCode != 200){
                return StatusCode(response.StatusCode, response);
            }

            //delete branch
             response = await _branchRepo.DeleteBranch(response.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    
       #region  get all Branches
        [HttpGet("")]
        public async Task<IActionResult> GetAllBranches()
        {
            ResponseModel response = await _branchRepo.GetAllBranches();
                return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region get Branch by id
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetBranchById(Guid branchId)
        {
            ResponseModel response = await _branchRepo.GetBranchById(branchId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region update Branch
        [CheckAccess]
        [HttpPut("")]
        public async Task<IActionResult> UpdateBranch([FromBody] BranchModel branch)
        {

           ResponseModel response = await _branchRepo.UpdateBranch(branch);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
