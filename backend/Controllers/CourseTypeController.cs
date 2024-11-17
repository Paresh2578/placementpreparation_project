using backend.Constant;
using backend.data.Repository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseTypeController : ControllerBase
    {
        private readonly CourseTypeRepo _courseTypeRepo;
        public CourseTypeController(CourseTypeRepo courseTypeRepo)
        {
            _courseTypeRepo = courseTypeRepo;
        }

        #region  Get All Course Types
        [HttpGet("")]
        public async Task<IActionResult> GetAllCourseTypes()
        {
            ResponseModel response = await _courseTypeRepo.GetAllCourseTypes();
            return StatusCode(response.StatusCode, response);
        }

        #endregion
   
        #region  Get Course Type By Id
        [HttpGet("{courseTypeId}")]
        public async Task<IActionResult> GetCourseTypeById(Guid courseTypeId)
        {
            ResponseModel response = await _courseTypeRepo.GetCourseTypeById(courseTypeId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region  Add Course Type
        [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddCourseType([FromBody] CourseTypeModel courseType)
        {
            ResponseModel response = await _courseTypeRepo.AddCourseType(courseType);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    
        #region  Delete Course Type
        [CheckAccess]
        [HttpDelete("{courseTypeId}")]
        public async Task<IActionResult> DeleteCourseType(Guid courseTypeId)
        {

            // Check if the course type is in use Course type id
            ResponseModel response = await _courseTypeRepo.GetCourseTypeById(courseTypeId);
            if(response.StatusCode != 200){
                return StatusCode(response.StatusCode, response);
            }

             response = await _courseTypeRepo.DeleteCourseType(response.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    
        #region  Update Course Type
        [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateCourseType([FromBody] CourseTypeModel courseType)
        {
            ResponseModel response = await _courseTypeRepo.UpdateCourseType(courseType);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
