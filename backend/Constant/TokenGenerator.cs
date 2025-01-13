using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Constant
{
    public class TokenGenerator
    {
        private static string _secret;

        // Method to initialize the secret key
        public static void Initialize(string secret)
        {
            _secret = secret ?? throw new ArgumentNullException(nameof(secret), "Secret key cannot be null");
        }

        public static string CreateToken(string userId)
        {
            if (string.IsNullOrEmpty(_secret))
                throw new InvalidOperationException("Secret key is not initialized. Call Initialize() first.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(_secret))
                throw new InvalidOperationException("Secret key is not initialized. Call Initialize() first.");


            if (string.IsNullOrEmpty(token))
                return false;

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
                var userData = claimsPrincipal.Claims
                    .ToDictionary(c => c.Type, c => c.Value); // Convert claims to a dictionary

                // Example: Access specific user data
                string userName = userData.ContainsKey(ClaimTypes.Name) ? userData[ClaimTypes.Name] : "Unknown";


                return claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Name);
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
   
         public static Dictionary<string,dynamic>? GetTokenToUserData(string token){
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
                if(!userData.ContainsKey("AdminUserId"))
                    return null;
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
