using backend.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.BAL
{
    public class CV{

        public static Dictionary<string,dynamic>? GetUserDataFromToken(string? token){
            if(string.IsNullOrEmpty(token)){
                return null;
            }
            Dictionary<string,dynamic>? userData = TokenGenerator.GetTokenToUserData(token);

            if(string.IsNullOrEmpty(userData["AdminUserId"]) || userData["IsAdmin"] == null){
                return null;
            }

            string adminId = userData["AdminUserId"];
            if(!Guid.TryParse(adminId, out Guid result)){
                return null;
            }
            return userData;
        }

        


    }
}