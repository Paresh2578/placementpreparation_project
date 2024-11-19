using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckAccess]
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
