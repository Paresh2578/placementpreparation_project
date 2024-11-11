using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
