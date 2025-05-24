
using Microsoft.AspNetCore.Mvc;


namespace Placement_Preparation.Areas.Student.Controllers
{
    [Area("Student")]
    public class CommunityController : Controller
    {

        #region List All Questions
        public IActionResult ListAllQuestions()
        {
            return View();
        }
        #endregion

        #region Question Details
        public IActionResult QuestionDetails()
        {
            return View();
        }
        #endregion

        #region Ask Question
        public IActionResult AskQuestion()
        {
            return View();
        }
        #endregion
    }
}