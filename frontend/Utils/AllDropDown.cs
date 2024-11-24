using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Utils
{
    public class AllDropDown
    {
        private readonly ApiClientService _apiClient;

        public AllDropDown(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<SelectListItem>> CourseType()
        {
            ApiResponseModel response = await _apiClient.GetAsync("CourseType");
             List<SelectListItem> courseTypeList = [];
                if(response.StatusCode == 200)
                {
                    // Deserialize the response data to get the list of CourseTypeModel
                   List<CourseTypeModel>  courseTypes =  JsonConvert.DeserializeObject<List<CourseTypeModel>>(response.Data!.ToString());

                   // Loop through the list of CourseTypeModel and add each item to the courseTypeList
                     foreach (var item in courseTypes)
                     {
                          courseTypeList.Add(new SelectListItem { Value = item.CourseTypeId.ToString(), Text = item.CourseTypeName });
                     }
                }
            return courseTypeList;
        }

        public async   Task<List<SelectListItem>> Branch()
        {
            ApiResponseModel response = await _apiClient.GetAsync("Branch");
             List<SelectListItem> branchList = [];
                if(response.StatusCode == 200)
                {
                    // Deserialize the response data to get the list of BranchModel
                   List<BranchModel>  branches =  JsonConvert.DeserializeObject<List<BranchModel>>(response.Data!.ToString());

                   // Loop through the list of BranchModel and add each item to the branchList
                     foreach (var item in branches)
                     {
                          branchList.Add(new SelectListItem { Value = item.BranchId.ToString(), Text = item.BranchName });
                     }
                }
            return branchList;
        }

        public async  Task<List<SelectListItem>> Course()
        {
            ApiResponseModel response = await _apiClient.GetAsync("Course");
             List<SelectListItem> courseList = [];
                if(response.StatusCode == 200)
                {
                    // Deserialize the response data to get the list of CourseModel
                   List<CourseModel>  courses =  JsonConvert.DeserializeObject<List<CourseModel>>(response.Data!.ToString());

                   // Loop through the list of CourseModel and add each item to the courseList
                     foreach (var item in courses)
                     {
                          courseList.Add(new SelectListItem { Value = item.CourseId.ToString(), Text = item.CourseName });
                     }
                }
            return courseList;
        }

        public async Task<List<SelectListItem>> DifficultyLevel()
        {
            ApiResponseModel response = await _apiClient.GetAsync("DifficultyLevel");
             List<SelectListItem> difficultyLevelList = [];
                if(response.StatusCode == 200)
                {
                    // Deserialize the response data to get the list of DifficultyLevelModel
                   List<DifficultyLevelModel>  courses =  JsonConvert.DeserializeObject<List<DifficultyLevelModel>>(response.Data!.ToString());

                   // Loop through the list of DifficultyLevelModel and add each item to the difficultyLevelList
                     foreach (var item in courses)
                     {
                          difficultyLevelList.Add(new SelectListItem { Value = item.DifficultyLevelId.ToString(), Text = item.DifficultyLevelName });
                     }
                }
            return difficultyLevelList;
        }

        public async Task<List<SelectListItem>> Topic()
        {
          ApiResponseModel response = await _apiClient.GetAsync("Topic");
             List<SelectListItem> topicList = [];
                if(response.StatusCode == 200)
                {
                    // Deserialize the response data to get the list of TopicModel
                   List<TopicModel>  topics =  JsonConvert.DeserializeObject<List<TopicModel>>(response.Data!.ToString());

                   // Loop through the list of TopicModel and add each item to the topicList
                     foreach (var item in topics)
                     {
                          topicList.Add(new SelectListItem { Value = item.TopicId.ToString(), Text = item.TopicName });
                     }
                }
            return topicList;
        }

        public async Task<List<SelectListItem>> SubTopic()
        {
            ApiResponseModel response = await _apiClient.GetAsync("SubTopic");
             List<SelectListItem> subTopicList = [];
                if(response.StatusCode == 200)
                {
                    // Deserialize the response data to get the list of SubTopicModel
                   List<SubTopicModel>  subTopics =  JsonConvert.DeserializeObject<List<SubTopicModel>>(response.Data!.ToString());

                   // Loop through the list of SubTopicModel and add each item to the subTopicList
                     foreach (var item in subTopics)
                     {
                          subTopicList.Add(new SelectListItem { Value = item.SubTopicId.ToString(), Text = item.SubTopicName });
                     }
                }
            return subTopicList;
        }
    }
}
