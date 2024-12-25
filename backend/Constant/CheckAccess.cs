using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Constant
{
    // Use ActionFilterAttribute for general action filtering
    public class CheckAccess : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the "Token" cookie exists
            if (!context.HttpContext.Request.Cookies.ContainsKey("token"))
            {
                // If not, return a forbidden result
               // context.Result = new UnauthorizedResult();
            }else
            {
                //// Retrieve the token from the cookie
                //var token = context.HttpContext.Request.Cookies["token"];

                //// Validate the token
                //if (string.IsNullOrEmpty(token) || !TokenGenerator.ValidateToken(token))
                //{
                //    // If token is invalid, return unauthorized result
                //    context.Result = new UnauthorizedResult();
                //}
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
