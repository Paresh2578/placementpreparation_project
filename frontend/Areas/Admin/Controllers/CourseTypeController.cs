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
