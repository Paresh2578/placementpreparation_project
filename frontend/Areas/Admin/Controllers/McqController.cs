using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class McqController : Controller
    {
        #region list of Mcq
        public IActionResult ListMcq()
        {
            List<McqModel> mcqList = new List<McqModel>
{
   
   };


            return View(mcqList);
        }
        #endregion

        #region add or Edit Mcq
        public IActionResult AddOrEditMcq(int? mcqId)
        {
            setDropDownsValue();

            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditMcq(McqModel mcq)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListMcq");
            }
            setDropDownsValue();
            return View(mcq);
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
