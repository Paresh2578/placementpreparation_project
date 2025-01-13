using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Placement_Preparation.BAL
{
    
        // Use ActionFilterAttribute for general action filtering
        public class MainAdminAccess : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                // Check if the "Token" cookie exists
                if (!context.HttpContext.Request.Cookies.ContainsKey("Token"))
                {
                    // If not, return a forbidden result
                    context.Result = new RedirectResult("~/Authentication/Auth/SignIn");
                }else{
                    Dictionary<string, dynamic>? userData =CV.TokenToUserData(context.HttpContext.Request.Cookies["Token"]!);

                    //  validate userData 
                    if(userData == null || userData.Count == 0 || !userData.ContainsKey("IsAdmin")){
                        context.Result = new RedirectResult("~/Authentication/Auth/SignIn");
                    }


                if (userData!["IsAdmin"] == false)
                {
                    context.Result = new RedirectResult("~/Admin/Common/Error403");
                }
            }

                base.OnActionExecuting(context);
            }

            // Optional: Set cache headers
            public override void OnResultExecuting(ResultExecutingContext context)
            {
                context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.HttpContext.Response.Headers["Expires"] = "-1";
                context.HttpContext.Response.Headers["Pragma"] = "no-cache";
                base.OnResultExecuting(context);
            }
        }
}
