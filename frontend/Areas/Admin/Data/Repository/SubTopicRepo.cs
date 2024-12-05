using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Placement_Preparation.Areas.Admin.Data.Interface;
using Placement_Preparation.Areas.Admin.Models;
using Newtonsoft.Json;

namespace Placement_Preparation.Areas.Admin.Data.Repository
{
    public class SubTopicRepo : SubTopicInterface
    {
        public readonly ApiClientService _apiClient;

        public SubTopicRepo(ApiClientService apiClient){
            _apiClient = apiClient;
        }

        

        public async Task<JsonResult> GetSubTopicsByTopicId(string topicId)
        {
            ApiResponseModel response = await _apiClient.GetAsync($"SubTopic/GetSubTopicsByTopicId/{topicId}");
            List<SubTopicModel> subTopicList = new List<SubTopicModel> { };

            // If the status code is not 200, return empty 
            if (response.StatusCode != 200)
            {
                return new JsonResult(subTopicList);
            }
            subTopicList = JsonConvert.DeserializeObject<List<SubTopicModel>>(response.Data!.ToString());
            return new JsonResult(subTopicList);
        }

        public async Task<JsonResult> GetSubTopicsByCourseId(string courseId)
        {
            ApiResponseModel response = await _apiClient.GetAsync($"SubTopic/GetSubTopicsByCourseId/{courseId}");
            List<SubTopicModel> subTopicList = new List<SubTopicModel> { };

            // If the status code is not 200, return empty 
            if (response.StatusCode != 200)
            {
                return new JsonResult(subTopicList);
            }
            subTopicList = JsonConvert.DeserializeObject<List<SubTopicModel>>(response.Data!.ToString());
            return new JsonResult(subTopicList);
        }
    }
}
 