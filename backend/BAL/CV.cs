using backend.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

    // "DBCS": "workstation id=PlacementPreparation.mssql.somee.com;packet size=4096;user id=paresh_SQLLogin_1;pwd=zdiv5wqrx4;data source=PlacementPreparation.mssql.somee.com;persist security info=False;initial catalog=PlacementPreparation;TrustServerCertificate=True"

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