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
    new CourseModel
    {
        CourseId = 1,
        CourseName = "Introduction to Programming",
        BranchId = 1, // Assume BranchId 1 corresponds to a specific branch in your database
        Branch = new BranchModel { BranchId = 1, BranchName = "Computer Science" },
        Description = "This course covers the basics of programming in various languages.",
        Img = "https://example.com/course1-image.jpg",
        CourseTypeId = 1, // Assume CourseTypeId 1 corresponds to a specific course type
        CourseType = new CourseTypeModel { CourseTypeId = 1, CourseTypeName = "Beginner" }
    },
    new CourseModel
    {
        CourseId = 2,
        CourseName = "Data Structures and Algorithms",
        BranchId = 1, // Again, adjust BranchId accordingly
        Branch = new BranchModel { BranchId = 1, BranchName = "Computer Science" },
        Description = "Learn about essential data structures and algorithms for problem-solving.",
        Img = "https://example.com/course2-image.jpg",
        CourseTypeId = 2, // Adjust CourseTypeId accordingly
        CourseType = new CourseTypeModel { CourseTypeId = 2, CourseTypeName = "Intermediate" }
    },
    new CourseModel
    {
        CourseId = 3,
        CourseName = "Web Development",
        BranchId = 2, // Example for a different branch
        Branch = new BranchModel { BranchId = 2, BranchName = "Information Technology" },
        Description = "A comprehensive guide to modern web development with hands-on projects.",
        Img = "https://example.com/course3-image.jpg",
        CourseTypeId = 1, // Another example of course type
        CourseType = new CourseTypeModel { CourseTypeId = 1, CourseTypeName = "Beginner" }
    }
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
