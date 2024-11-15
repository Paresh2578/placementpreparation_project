using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        #region list of Topic
        public IActionResult ListQuestion()
        {
            List<QuestionModel> questionList = new List<QuestionModel>
{
    new QuestionModel
    {
        QuestionId = 1,
        SubTopicId = 1, // Assuming this question belongs to SubTopicId 1
        SubTopic = new SubTopicModel
        {
            SubTopicId = 1,
            SubTopicName = "Introduction to Algorithms",
            TopicId = 1,
            Topic = new TopicModel
            {
                TopicId = 1,
                TopicName = "Algorithms",
                CourseId = 1,
                Content = "This topic covers the basics of algorithms.",
                DifficultyLevelId = 2
            }
        },
        Question = "What is an algorithm?",
        QuestionAnswer = "An algorithm is a step-by-step procedure to solve a problem or perform a task."
    },
    new QuestionModel
    {
        QuestionId = 2,
        SubTopicId = 2, // Assuming this question belongs to SubTopicId 2
        SubTopic = new SubTopicModel
        {
            SubTopicId = 2,
            SubTopicName = "Data Structures Overview",
            TopicId = 2,
            Topic = new TopicModel
            {
                TopicId = 2,
                TopicName = "Data Structures",
                CourseId = 1,
                Content = "This topic includes an overview of common data structures.",
                DifficultyLevelId = 1
            }
        },
        Question = "What is a data structure?",
        QuestionAnswer = "A data structure is a particular way of organizing and storing data so that it can be accessed and modified efficiently."
    },
    new QuestionModel
    {
        QuestionId = 3,
        SubTopicId = 1, // Assuming this question also belongs to SubTopicId 1
        SubTopic = new SubTopicModel
        {
            SubTopicId = 1,
            SubTopicName = "Introduction to Algorithms",
            TopicId = 1,
            Topic = new TopicModel
            {
                TopicId = 1,
                TopicName = "Algorithms",
                CourseId = 1,
                Content = "This topic covers the basics of algorithms.",
                DifficultyLevelId = 2
            }
        },
        Question = "What are the characteristics of a good algorithm?",
        QuestionAnswer = "A good algorithm is correct, efficient, simple, unambiguous, and has well-defined inputs and outputs."
    }
};


            return View(questionList);
        }
        #endregion

        #region add or Edit Question
        public IActionResult AddOrEditQuestion(int? questionId)
        {
            setDropDownsValue();

            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditQuestion(QuestionModel questions)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListQuestion");
            }
            setDropDownsValue();
            return View(questions);
        }
        #endregion

        #region set Topic and Sub Topic DropDown Value
        [NonAction]
        public void setDropDownsValue()
        {
            ViewBag.TopicList = AllDropDown.Topic();
            ViewBag.SubTopicList = AllDropDown.SubTopic();
        }
        #endregion
    }
}
