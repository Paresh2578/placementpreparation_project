using backend.BAL;
using backend.data.Interface;
using backend.data.Repository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseTypeController : ControllerBase
    {
        private readonly CourseTypeInterface _courseTypeInterface;
        public CourseTypeController(CourseTypeInterface courseTypeInterface)
        {
            _courseTypeInterface = courseTypeInterface;
        }

        #region  Get All Course Types
        [HttpGet]
        public async Task<IActionResult> GetAllCourseTypes()
        {
            ResponseModel response = await _courseTypeInterface.GetAllCourseTypes();
            return StatusCode(response.StatusCode, response);
        }

        #endregion
   
        #region  Get Course Type By Id
        [HttpGet("{courseTypeId}")]
        public async Task<IActionResult> GetCourseTypeById(Guid courseTypeId)
        {
            ResponseModel response = await _courseTypeInterface.GetCourseTypeById(courseTypeId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region  Add Course Type
        [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddCourseType([FromBody] CourseTypeModel courseType)
        {
            ResponseModel response = await _courseTypeInterface.AddCourseType(courseType);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    
        #region  Delete Course Type
        [CheckAccess]
        [HttpDelete("{courseTypeId}")]
        public async Task<IActionResult> DeleteCourseType(Guid courseTypeId)
        {

            // Check if the course type is in use Course type id
            ResponseModel response = await _courseTypeInterface.GetCourseTypeById(courseTypeId);
            if(response.StatusCode != 200){
                return StatusCode(response.StatusCode, response);
            }

             response = await _courseTypeInterface.DeleteCourseType(response.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    
        #region  Update Course Type
        [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateCourseType([FromBody] CourseTypeModel courseType)
        {
            ResponseModel response = await _courseTypeInterface.UpdateCourseType(courseType);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

         #region Delete Multiple Courses Type
        [HttpDelete("DeleteMultiple")]
        [CheckAccess]
        public async Task<IActionResult> DeleteMultipleMcq([FromBody] List<Guid> courseTypeIds){
            var response = await _courseTypeInterface.DeleteMultipleCourseType(courseTypeIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion
    }
}
