using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Constant
{
    public class TokenGenerator
    {
        private static String _secret;        // Method to initialize the secret key
        public static void Initialize(string secret)
        {
            _secret = secret;
        }

        public static string CreateToken(String userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateToken(String token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Skip issuer validation if not set
                    ValidateAudience = false, // Skip audience validation if not set
                    ClockSkew = TimeSpan.Zero // Disable clock skew for testing
                }, out SecurityToken validatedToken);

                // Extract the `Name` claim (where userId is stored)
                var userId = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;

                return !string.IsNullOrEmpty(userId);
            }
            catch
            {
                // Return null if token is invalid
                return false;
            }
        }
    }
}
