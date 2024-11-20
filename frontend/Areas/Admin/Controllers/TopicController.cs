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
