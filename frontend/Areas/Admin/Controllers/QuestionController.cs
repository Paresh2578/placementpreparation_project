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
