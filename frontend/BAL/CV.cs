namespace Placement_Preparation.BAL
{
    public class CV
    {
         private static IHttpContextAccessor _httpContextAccessor;

        static CV()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

          public static string? UserName()
        {
            return _httpContextAccessor.HttpContext!.Session.GetString("UserName");
        }

        public static string? UserEmail()
        {
            return _httpContextAccessor.HttpContext!.Session.GetString("Email");
        }
    }
}