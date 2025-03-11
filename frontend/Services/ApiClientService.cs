using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Frontend.Models;

namespace Frontend.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public ApiClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Redirect to Login
        
        private void RedirectToLogin()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Response.Redirect("/Authentication/Auth/SignIn"); 
            }
        }
        #endregion

        #region Common GetAsync 
        public async Task<ApiResponseModel> GetAsync(string endpoint)
        {
            try{
                var response = await _httpClient.GetAsync(endpoint);

                 // many request send to server in a short time
                if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new ApiResponseModel {StatusCode = 429,Message = "Too many requests send in a short time. Plz try again after 5 secound"};
                }

                var responseContent = await response.Content.ReadAsStringAsync();

				// Redirect to login if unauthorized
				if (response.ReasonPhrase == "Unauthorized")
				{
					RedirectToLogin();
				}

				ApiResponseModel? apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);
                if(apiResponseModel is not null && apiResponseModel.StatusCode != 0)
                {
                    return apiResponseModel;    
                }
                 // Redirect to login if unauthorized
                if(apiResponseModel.StatusCode == 403 || response.ReasonPhrase == "Unauthorized")
                {
                    RedirectToLogin();
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


				// many request send to server in a short time
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests || response.ReasonPhrase == "Too Many Requests")
				{
					return new ApiResponseModel { StatusCode = 429, Message = "Too many requests send in a short time. Plz try again after 5 secound" };
				}

                // Redirect to login if unauthorized
                if(response.ReasonPhrase == "Unauthorized")
                {
                    RedirectToLogin();
                }

				var responseContent = await response.Content.ReadAsStringAsync();

                ApiResponseModel? apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);

                if(apiResponseModel != null && apiResponseModel.StatusCode != 0)
                {
                    return apiResponseModel;
                }

                // Redirect to login if unauthorized
                if(apiResponseModel.StatusCode == 403 || response.ReasonPhrase == "Unauthorized")
                {
                    RedirectToLogin();
                }


                return new ApiResponseModel {StatusCode = (int)response.StatusCode,Message = response.ReasonPhrase};
            }catch(Exception ex){
                return new ApiResponseModel {StatusCode = 500,Message = ex.Message};
            }
      }
      #endregion

      #region Common PutAsync
      public async Task<ApiResponseModel> PutAsync(string endpoint , object data){
            try{
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endpoint,content);

                 // many request send to server in a short time
                if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new ApiResponseModel {StatusCode = 429,Message = "Too many requests send in a short time. Plz try again after 5 secound"};
                }

				// Redirect to login if unauthorized
				if (response.ReasonPhrase == "Unauthorized")
				{
					RedirectToLogin();
				}


                var responseContent = await response.Content.ReadAsStringAsync();

                ApiResponseModel? apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);

                if(apiResponseModel != null && apiResponseModel.StatusCode != 0)
                {
                    return apiResponseModel;
                }
                 // Redirect to login if unauthorized
                if(apiResponseModel.StatusCode == 403 || response.ReasonPhrase == "Unauthorized")
                {
                    RedirectToLogin();
                }

                
                // many request send to server in a short time
                if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new ApiResponseModel {StatusCode = 429,Message = "Too many requests send in a short time. Plz try again after 5 secound"};
                }
                return new ApiResponseModel {StatusCode = (int)response.StatusCode,Message = response.ReasonPhrase};
            }catch(Exception ex){
                return new ApiResponseModel {StatusCode = 500,Message = ex.Message};
            }
      }
      #endregion
   
   
      #region Common DeleteAsync
        public async Task<ApiResponseModel> DeleteAsync(string endpoint){
                try{
                    var response = await _httpClient.DeleteAsync(endpoint);
                    var responseContent = await response.Content.ReadAsStringAsync();
    
                    ApiResponseModel? apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);
    
                    if(apiResponseModel != null && apiResponseModel.StatusCode != 0)
                    {
                        return apiResponseModel;
                    }
                     // Redirect to login if unauthorized
                if(apiResponseModel.StatusCode == 403 || response.ReasonPhrase == "Unauthorized")
                {
                    RedirectToLogin();
                }

                
                // many request send to server in a short time
                if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new ApiResponseModel {StatusCode = 429,Message = "Too many requests send in a short time. Plz try again after 5 secound"};
                }

                    return new ApiResponseModel {StatusCode = (int)response.StatusCode,Message = response.ReasonPhrase};
                }catch(Exception ex){
                    return new ApiResponseModel {StatusCode = 500,Message = ex.Message};
                }
      }
        #endregion
    
       
       #region  Delete Multiple
        public async Task<ApiResponseModel> DeleteMultipleAsync(string endpoint , object data){
            try{
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(_httpClient.BaseAddress + endpoint),
                    Content = content
                };
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                ApiResponseModel? apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);

                if(apiResponseModel != null && apiResponseModel.StatusCode != 0)
                {
                    return apiResponseModel;
                }
                 // Redirect to login if unauthorized
                if(apiResponseModel.StatusCode == 403 || response.ReasonPhrase == "Unauthorized")
                {
                    RedirectToLogin();
                }

                
                // many request send to server in a short time
                if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new ApiResponseModel {StatusCode = 429,Message = "Too many requests send in a short time. Plz try again after 5 secound"};
                }

                return new ApiResponseModel {StatusCode = (int)response.StatusCode,Message = response.ReasonPhrase};
            }catch(Exception ex){
                return new ApiResponseModel {StatusCode = 500,Message = ex.Message};
            }
        }
        #endregion

    #region image upload
        public async Task<ApiResponseModel> UploadImage(IFormFile file)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(file.OpenReadStream())
                {
                    Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                    }
                }, "image", file.FileName);

                var response = await _httpClient.PostAsync("ImageUpload", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                ApiResponseModel? apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);

                if(apiResponseModel != null && apiResponseModel.StatusCode != 0)
                {
                    return apiResponseModel;
                }
                 // Redirect to login if unauthorized
                if(apiResponseModel.StatusCode == 403 || response.ReasonPhrase == "Unauthorized")
                {
                    RedirectToLogin();
                }

                
                // many request send to server in a short time
                if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new ApiResponseModel {StatusCode = 429,Message = "Too many requests send in a short time. Plz try again after 5 secound"};
                }

                   return new ApiResponseModel {StatusCode = (int)response.StatusCode,Message = response.ReasonPhrase};
            }
            catch (Exception ex)
            {
                  return new ApiResponseModel {StatusCode = 500,Message = ex.Message};
            }
        }
        #endregion
    }
}
