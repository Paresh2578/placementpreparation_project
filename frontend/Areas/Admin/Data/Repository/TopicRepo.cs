using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Data.Interface;
using Placement_Preparation.Areas.Admin.Models;
using Newtonsoft.Json;

namespace Placement_Preparation.Areas.Admin.Data.Repository
{
    public class TopicRepo : TopicInterface
    {
        public readonly ApiClientService _apiClient;

        public TopicRepo(ApiClientService apiClient){
            _apiClient = apiClient;
        }

        

        public async Task<JsonResult> GetTopicsByCourseId(string courseId)
        {
            ApiResponseModel response = await _apiClient.GetAsync($"Topic/GetTopicsByCourseId/{courseId}");
            List<TopicModel> topicList = new List<TopicModel> { };

            // If the status code is not 200, return empty 
            if (response.StatusCode != 200)
            {
                return new JsonResult(topicList);
            }
            topicList = JsonConvert.DeserializeObject<List<TopicModel>>(response.Data!.ToString());
            return new JsonResult(topicList);
        }

        public async Task<JsonResult> GetTopicsLengthByCourseId(string courseId)
        {
            ApiResponseModel response = await _apiClient.GetAsync($"Topic/GetTopicsLengthByCourseId/{courseId}");
            return new JsonResult(response.Data);
        }
    }
}
 