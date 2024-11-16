using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {

        #region list of Branch
        public IActionResult List()
        {
            List<BranchModel> branchList = new List<BranchModel>
                {
                    new BranchModel { BranchId = 1, BranchName = "Branch 1 Name" },
                    new BranchModel { BranchId = 2, BranchName = "Branch 2 Name" },
                    new BranchModel { BranchId = 3, BranchName = "Branch 3 Name" },
                    new BranchModel { BranchId = 4, BranchName = "Branch 4 Name" },
                    new BranchModel { BranchId = 5, BranchName = "Branch 5 Name" },
                    new BranchModel { BranchId = 6, BranchName = "Branch 6 Name" },
                    new BranchModel { BranchId = 7, BranchName = "Branch 7 Name" },
                    new BranchModel { BranchId = 8, BranchName = "Branch 8 Name" },
                    new BranchModel { BranchId = 9, BranchName = "Branch 9 Name" },
                    new BranchModel { BranchId = 10, BranchName = "Branch 10 Name" },

                };
            return View(branchList);
        }
        #endregion


        #region add or Edit Branch
        public IActionResult AddOrEditBranch(int? branchId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditBranch(BranchModel branch)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("List");
            }
            return View();
        }
        #endregion
    }
}
