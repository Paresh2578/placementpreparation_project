using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseInterface _courseInterface;

        public CourseController(CourseInterface courseInterface)
        {
            _courseInterface = courseInterface;
        }
        
        #region  Get All Courses
        [HttpGet]
        public async Task<IActionResult> GetAllCourses([FromQuery] int? limit , [FromQuery] string? courseType)
        {
            var response = await _courseInterface.GetAllCourses(limit , courseType);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Get Course By Id
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(Guid courseId)
        {
            var response = await _courseInterface.GetCourseById(courseId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
         
        #region Get Course detais with all subTopic List By Course Id
        [HttpGet("GetCourseDetailsById/{courseId}")]
        public async Task<IActionResult> GetCourseDetailsById(Guid courseId)
        {
            var response = await _courseInterface.GetCourseDetailsById(courseId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Add Course
        // [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseModel course)
        {
            var response = await _courseInterface.AddCourse(course);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Update Course
        // [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseModel course)
        { 
           ResponseModel response = await _courseInterface.UpdateCourse(course);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Delete Course
        // [CheckAccess]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var course = await _courseInterface.GetCourseById(courseId);
            if (course.StatusCode != 200)
            {
                return StatusCode(course.StatusCode, course);
            }

            var response = await _courseInterface.DeleteCourse(course.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Course Dropdown
        [HttpGet("dropdown")]
        public async Task<IActionResult> CourseDropdown()
        {
            var response = await _courseInterface.CourseDropdown();
            return StatusCode(response.StatusCode, response);
        }
        #endregion

         #region Delete Multiple Courses
        [HttpDelete("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultipleMcq([FromBody] List<Guid> courseIds){
            var response = await _courseInterface.DeleteMultipleCourse(courseIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion

        #region Get Course By Branch , Course Type
        [HttpGet("GetCoursesByBranchAndCourseType")]
        public async Task<IActionResult> GetCoursesByBranchAndCourseType([FromQuery] Guid? branchId , [FromQuery] Guid? courseTypeId)
        {
            var response = await _courseInterface.GetCoursesByBranchAndCourseType(branchId , courseTypeId);
            return StatusCode(response.StatusCode , response);
        }
        #endregion

        
    }
}
