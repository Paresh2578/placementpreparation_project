using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : Controller
    {
        #region  Error 403 Forbidden Page
        public IActionResult Error403()
        {
            return View();
        }
        #endregion

        #region  Approval Status Page
        public IActionResult ApprovalStatus()
        {
            return View();
        }
        #endregion
    }
} 