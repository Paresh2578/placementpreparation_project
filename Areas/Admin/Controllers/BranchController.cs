using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
