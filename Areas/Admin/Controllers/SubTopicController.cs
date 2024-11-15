using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubTopicController : Controller
    {
        #region list of Sub Topic
        public IActionResult ListSubTopic()
        {
            List<SubTopicModel> subTopicList = new List<SubTopicModel>
{
    new SubTopicModel
    {
        SubTopicId = 1,
        SubTopicName = "Introduction to Algorithms",
        TopicId = 1, // Assuming it belongs to TopicId 1
        Topic = new TopicModel
        {
            TopicId = 1,
            TopicName = "Algorithms",
            CourseId = 1,
            Content = "This topic covers the basics of algorithms.",
            DifficultyLevelId = 2
        }
    },
    new SubTopicModel
    {
        SubTopicId = 2,
        SubTopicName = "Data Structures Overview",
        TopicId = 2, // Assuming it belongs to TopicId 2
        Topic = new TopicModel
        {
            TopicId = 2,
            TopicName = "Data Structures",
            CourseId = 1,
            Content = "This topic includes an overview of common data structures.",
            DifficultyLevelId = 1
        }
    },
    new SubTopicModel
    {
        SubTopicId = 3,
        SubTopicName = "Advanced Sorting Techniques",
        TopicId = 1, // Assuming it belongs to TopicId 1
        Topic = new TopicModel
        {
            TopicId = 1,
            TopicName = "Algorithms",
            CourseId = 1,
            Content = "This topic discusses advanced sorting algorithms.",
            DifficultyLevelId = 3
        }
    }
};


            return View(subTopicList);
        }
        #endregion

        #region add or Edit Sub Topic
        public IActionResult AddOrEditSubTopic(int? subTopicId)
        {
            setDropDownsValue();

            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditSubTopic(SubTopicModel subTopic)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListSubTopic");
            }
            setDropDownsValue();
            return View(subTopic);
        }
        #endregion

        #region set Topic DropDown Value
        [NonAction]
        public void setDropDownsValue()
        {
            ViewBag.topicList = AllDropDown.Topic();
        }
        #endregion
    }
}
