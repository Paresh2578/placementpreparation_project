using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Controllers
{
	public class CommonController : Controller
	{
          #region Page Not Found
        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            return statusCode switch
            {
                404 => View("NotFound"),// Return the NotFound view for 404 errors
                _ => View("Error"),// Return the Error view for all other errors
            };
        }
        #endregion
    }
}