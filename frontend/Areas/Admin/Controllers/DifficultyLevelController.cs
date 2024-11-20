using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DifficultyLevelController : Controller
    {

        #region list of Difficulty Level 
        public IActionResult List()
        {
            List<DifficultyLevelModel> difficultyLevelList = new List<DifficultyLevelModel>
                {
    };
            return View(difficultyLevelList);
        }
        #endregion


        #region add or Edit Branch
        public IActionResult AddOrEditDifficultyLevel(int? branchId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditDifficultyLevel(DifficultyLevelModel difficultyLevel)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                Console.WriteLine("%%%%%%%%%%%%%%%%%% " + difficultyLevel.DifficultyLevelName);
                return RedirectToAction("List");
            }
            return View(difficultyLevel);
        }
        #endregion
    }
}
