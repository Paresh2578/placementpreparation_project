namespace Placement_Preparation.BAL
{
    public class CV
    {
         private static IHttpContextAccessor _httpContextAccessor;

        static CV()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public static void SetEmail(string email){
            _httpContextAccessor.HttpContext!.Session.SetString("Email", email);
        }

        public static string GetEmail(){
            return _httpContextAccessor.HttpContext!.Session.GetString("Email")!;
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