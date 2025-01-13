using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Placement_Preparation.BAL
{
    public class CV
    {
         private static IHttpContextAccessor _httpContextAccessor;
         private static IConfiguration _configartion;

        static CV()
        {
            _httpContextAccessor = new HttpContextAccessor();
            _configartion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public static void SetEmail(string email){
            _httpContextAccessor.HttpContext!.Session.SetString("Email", email);
        }

        public static string GetEmail(){
            return _httpContextAccessor.HttpContext!.Session.GetString("Email")!;
        }

        public static void SetUserName(string userName){
            _httpContextAccessor.HttpContext!.Session.SetString("UserName", userName);
        }

          public static string GetId(){
            return _httpContextAccessor.HttpContext!.Session.GetString("Id")!;
        }


          public static string? UserName()
        {
            return _httpContextAccessor.HttpContext!.Session.GetString("UserName");
        }

        public static string? UserEmail()
        {   
            return _httpContextAccessor.HttpContext!.Session.GetString("Email");
        }

        public static void SetIsAdminAndStudentApprovalStatusAndId(bool isAdmin , string approvalStatus , string id)
        {
            _httpContextAccessor.HttpContext!.Session.SetString("IsAdmin", isAdmin.ToString());
            _httpContextAccessor.HttpContext!.Session.SetString("ApprovalStatus", approvalStatus);
            _httpContextAccessor.HttpContext!.Session.SetString("Id", id);
        }

        public static bool GetIsAdmin()
        {
            return Convert.ToBoolean(_httpContextAccessor.HttpContext!.Session.GetString("IsAdmin"));
        }

        public static string GetApprovalStatus()
        {
            return _httpContextAccessor.HttpContext!.Session.GetString("ApprovalStatus");
        }

        public static Dictionary<string,dynamic>? TokenToUserData(string token){
            string? _secret = _configartion.GetSection("Jwt")["SecretKey"];

           if (string.IsNullOrEmpty(_secret))
                 throw new InvalidOperationException("Secret key is not initialized.");


            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);


            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Skip issuer validation
                    ValidateAudience = false, // Skip audience validation
                    ClockSkew = TimeSpan.Zero // Disable clock skew
                }, out _);

                // Extract user data from claims
                var userDataInClaims = claimsPrincipal.Claims
                    .ToDictionary(c => c.Type, c => c.Value); // Convert claims to a dictionary

                // Example: Access specific user data
                string userDataInString = userDataInClaims.ContainsKey(ClaimTypes.Name) ? userDataInClaims[ClaimTypes.Name] : "Unknown";

                

                if(userDataInString == "Unknown" || string.IsNullOrEmpty(userDataInString) || !claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Name))
                    return null;

               Dictionary<string,dynamic>? userData = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(userDataInString);

               if(userData == null) return null;// ApproveStatus, 

                // set and check Admin status data
                if(!userData.ContainsKey("IsAdmin") || !userData.ContainsKey("ApproveStatus") || !userData.ContainsKey("AdminUserId"))
                    return null;

                SetIsAdminAndStudentApprovalStatusAndId(userData["IsAdmin"],userData["ApproveStatus"],userData["AdminUserId"].ToString());    
                
                return userData;    
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }

        }
    }
}