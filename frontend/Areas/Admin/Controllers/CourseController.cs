using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;
using System.Reflection.Metadata.Ecma335;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        #region list of Course
        public IActionResult ListCourse()
        {
            List<CourseModel> courseList = new List<CourseModel>
{
 
};
            return View(courseList);
        }
        #endregion

        #region add or Edit Course
        public IActionResult AddOrEditCourse(int? courseId)
        {
            setDropDownsValue();

            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditCourse(CourseModel course)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListCourse");
            }
            setDropDownsValue();
            return View(course);
        }
        #endregion

        #region set Course Type and Branch DropDown Value
        [NonAction]
        public void setDropDownsValue()
        {
            ViewBag.CourseTypeList = AllDropDown.CourseType();
            ViewBag.BranchList = AllDropDown.Branch();
        }
        #endregion
    }
}
