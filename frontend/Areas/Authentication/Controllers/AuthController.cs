using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
