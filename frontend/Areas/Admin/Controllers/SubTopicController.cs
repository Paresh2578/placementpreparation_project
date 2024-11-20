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
