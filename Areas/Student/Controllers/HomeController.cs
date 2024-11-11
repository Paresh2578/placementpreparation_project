using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Student.Controllers
{
    [Area("Student")]
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
