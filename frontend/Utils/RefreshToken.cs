using Frontend.Models;
using Frontend.Services;
using Newtonsoft.Json;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Utils{
    public class RefreshToken{
       private static readonly ApiClientService  _apiClientService;
        static RefreshToken()
        {
            _apiClientService = new ApiClientService(new HttpClient());
        }
        public static  void Refresh(){// 
          
        }
    }
}