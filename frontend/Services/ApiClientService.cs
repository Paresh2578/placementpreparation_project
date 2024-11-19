using System.Text;
using Newtonsoft.Json;
using Frontend.Models;

namespace Frontend.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;

        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       # region Common GetAsync Method
        public async Task<ApiResponseModel> GetAsync(string endpoint)
        {
            try{
                var response = await _httpClient.GetAsync(endpoint);
                if(response.IsSuccessStatusCode){
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ApiResponseModel>(content)!;
                }
                return new ApiResponseModel {StatusCode = (int)response.StatusCode,Message = response.ReasonPhrase};
            }catch(Exception ex){
                return new ApiResponseModel {StatusCode = 500,Message = ex.Message};
            }
        }
        #endregion
   
      #region Common PostAsync
      public async Task<ApiResponseModel> PostAsync(string endpoint , object data){
            try{
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint,content);
                var responseContent = await response.Content.ReadAsStringAsync();

                ApiResponseModel? apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);

                if(apiResponseModel is not null)
                {
                    return apiResponseModel;
                }
                return new ApiResponseModel {StatusCode = (int)response.StatusCode,Message = response.ReasonPhrase};
            }catch(Exception ex){
                return new ApiResponseModel {StatusCode = 500,Message = ex.Message};
            }
      }
      #endregion
    }
}
