using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicController : Controller
    {

        #region list of Topic
        public IActionResult ListTopic()
        {
            List<TopicModel> topicList = new List<TopicModel>
{
    new TopicModel
    {
        TopicId = 1,
        TopicName = "Introduction to Programming",
        CourseId = 1,
        Course = new CourseModel { CourseId = 1, CourseName = "Computer Science" },
        Content = "This topic covers the basics of programming, including syntax and structure.",
        DifficultyLevelId = 1,
        DifficultyLevel = new DifficultyLevelModel { DifficultyLevelId = 1, DifficultyLevelName = DifficultyLevelName.Hard  }
    },
    new TopicModel
    {
        TopicId = 2,
        TopicName = "Data Structures",
        CourseId = 1,
        Course = new CourseModel { CourseId = 1, CourseName = "Computer Science" },
        Content = "Learn about different data structures such as arrays, lists, and trees.",
        DifficultyLevelId = 2,
        DifficultyLevel = new DifficultyLevelModel { DifficultyLevelId = 2,DifficultyLevelName = DifficultyLevelName.Medium }
    },
    new TopicModel
    {
        TopicId = 3,
        TopicName = "Algorithms",
        CourseId = 1,
        Course = new CourseModel { CourseId = 1, CourseName = "Computer Science" },
        Content = "An introduction to algorithms including sorting and searching.",
        DifficultyLevelId = 3,
        DifficultyLevel = new DifficultyLevelModel { DifficultyLevelId = 3, DifficultyLevelName = DifficultyLevelName.Easy }
    },
};

            return View(topicList);
        }
        #endregion

        #region add or Edit Topic
        public IActionResult AddOrEditTopic(int? topicId)
        {
            setDropDownsValue();

            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditTopic(TopicModel topic)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListTopic");
            }
            setDropDownsValue();
            return View(topic);
        }
        #endregion

        #region set Course  and Difficulty Level DropDown Value
        [NonAction]
        public void setDropDownsValue()
        {
            ViewBag.CourseList = AllDropDown.Course();
            ViewBag.difficultyLevelList = AllDropDown.DifficultyLevel();
        }
        #endregion
    }
}
