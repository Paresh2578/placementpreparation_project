using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseTypeController : Controller
    {

        #region list of  CourseType 
        public IActionResult ListCourseType()
        {
            List<CourseTypeModel> courseTypeList = new List<CourseTypeModel>
            {
                new CourseTypeModel { CourseTypeId = 1, CourseTypeName = "Computer Science" },
                new CourseTypeModel { CourseTypeId = 2, CourseTypeName = "Mechanical Engineering" },
                new CourseTypeModel { CourseTypeId = 3, CourseTypeName = "Civil Engineering" },
                new CourseTypeModel { CourseTypeId = 4, CourseTypeName = "Electrical Engineering" },
                new CourseTypeModel { CourseTypeId = 5, CourseTypeName = "Business Administration" },
                new CourseTypeModel { CourseTypeId = 6, CourseTypeName = "Data Science" },
                new CourseTypeModel { CourseTypeId = 7, CourseTypeName = "Psychology" },
                new CourseTypeModel { CourseTypeId = 8, CourseTypeName = "Marketing" },
                new CourseTypeModel { CourseTypeId = 9, CourseTypeName = "Graphic Design" },
                new CourseTypeModel { CourseTypeId = 10, CourseTypeName = "Artificial Intelligence" }
            };

            return View(courseTypeList);
        }
        #endregion


        #region add or Edit Branch
        public IActionResult AddOrEditCourseType(int? courseTypeId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditCourseType(CourseTypeModel courseType)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListCourseType");
            }
            return View(courseType);
        }
        #endregion
    }
}
