using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Controllers;
using Placement_Preparation.Areas.Admin.Data.Interface;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Utils
{
    public class AllDropDown
    {
        private readonly ApiClientService _apiClient;
        private readonly TopicInterface _topicInterface;
        private readonly SubTopicInterface _subTopicInterface;

        public AllDropDown(ApiClientService apiClient , TopicInterface topicInerface , SubTopicInterface subTopicInterface)
        {
            _apiClient = apiClient;
            _topicInterface = topicInerface;
            _subTopicInterface = subTopicInterface;
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

                Console.WriteLine(branchList);
            return branchList;
        }

        public async  Task<List<SelectListItem>> Course()
        {
            ApiResponseModel response = await _apiClient.GetAsync("Course/dropdown");
             List<SelectListItem> courseList = [];
                if(response.StatusCode == 200)
                {
                    // Deserialize the response data to get the list of CourseModel
                   List<CourseModel>  courses =  JsonConvert.DeserializeObject<List<CourseModel>>(response.Data!.ToString());

                   // Loop through the list of CourseModel and add each item to the courseList
                     foreach (var item in courses)
                     {
                          courseList.Add(new SelectListItem { Value = item.CourseId.ToString(), Text = item.CourseName.ToString() });
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

        public async Task<List<SelectListItem>> GetAllTopicsByCourseId(string courseId)
        {
           JsonResult jsonResult = await _topicInterface.GetTopicsByCourseId(courseId:courseId);

           // SelectListItems
           List<SelectListItem> topicSelectLists = new List<SelectListItem>{};

           foreach(TopicModel topic in jsonResult.Value as List<TopicModel>){
            topicSelectLists.Add(new SelectListItem(){Text=topic.TopicName , Value=topic.TopicId.ToString()});
           }

           return topicSelectLists;
        }

        public async Task<List<SelectListItem>> GetAllSubTopicsByTopicId(string topicId)
        {
           JsonResult jsonResult = await _subTopicInterface.GetSubTopicsByTopicId(topicId:topicId);

           // SelectListItems
           List<SelectListItem> subTopicSelectLists = new List<SelectListItem>{};

           foreach(SubTopicModel subTopic in jsonResult.Value as List<SubTopicModel>){
            subTopicSelectLists.Add(new SelectListItem(){Text=subTopic.SubTopicName , Value=subTopic.SubTopicId.ToString()});
           }

           return subTopicSelectLists;
        }
   
       public async Task<List<SelectListItem>> GetTopicLengthByCourseId(string courseId){
        JsonResult jsonResult = await _topicInterface.GetTopicsLengthByCourseId(courseId:courseId);

        dynamic jsonValue = jsonResult.Value;

        int length = jsonValue.length;

        List<SelectListItem> topicLevel = new List<SelectListItem>{};
       
        for(int i=1;i<=length;i++){
            topicLevel.Add(new SelectListItem(){Text=i.ToString(),Value=i.ToString()});
        }

        return topicLevel;
       }
        public async Task<List<SelectListItem>> GetSubTopicLengthByTopicId(string topicId){
        JsonResult jsonResult = await _subTopicInterface.GetSubTopicLengthByTopicId(topicId:topicId);

        dynamic jsonValue = jsonResult.Value;

        int length = jsonValue.length;

        List<SelectListItem> topicLevel = new List<SelectListItem>{};
       
        for(int i=1;i<=length;i++){
            topicLevel.Add(new SelectListItem(){Text=i.ToString(),Value=i.ToString()});
        }

        return topicLevel;
       }

       public async Task<Dictionary<string,List<SelectListItem>>> GetQuestionCompanyAndTechStack(){
        ApiResponseModel response = await _apiClient.GetAsync("Question/GetAllUniqueCompanyNamesAndTechStack");
        Dictionary<string,List<SelectListItem>> companyAndTechStack = new Dictionary<string,List<SelectListItem>>{};

        if(response.StatusCode != 200){
            return companyAndTechStack;
        }

        Dictionary<string,List<string>> companyAndTechStackDict = JsonConvert.DeserializeObject<Dictionary<string,List<string>>>(response.Data!.ToString());

        List<SelectListItem> companyList = new List<SelectListItem>{
        new SelectListItem(){Text="All Company",Value=""}
        };
        List<SelectListItem> techStackList = new List<SelectListItem>{
        new SelectListItem(){Text="All Tech Stack",Value=""}
        };

        foreach(string? company in companyAndTechStackDict["companyNames"]){
            if(string.IsNullOrEmpty(company)){
                continue;
            }
            companyList.Add(new SelectListItem(){Text=company,Value=company});
        }

        foreach(string? techStack in companyAndTechStackDict["techStacks"]){
            if(string.IsNullOrEmpty(techStack)){
                continue;
            }
            techStackList.Add(new SelectListItem(){Text=techStack,Value=techStack});
        }

        companyAndTechStack.Add("companyNames",companyList);
        companyAndTechStack.Add("techStacks",techStackList);

        return  companyAndTechStack;
       }

          public async Task<Dictionary<string,List<SelectListItem>>> GetMcqCompanyAndTechStack(){
        ApiResponseModel response = await _apiClient.GetAsync("Mcq/GetAllUniqueCompanyNamesAndTechStack");
        Dictionary<string,List<SelectListItem>> companyAndTechStack = new Dictionary<string,List<SelectListItem>>{};

        if(response.StatusCode != 200){
            return companyAndTechStack;
        }

        Dictionary<string,List<string>> companyAndTechStackDict = JsonConvert.DeserializeObject<Dictionary<string,List<string>>>(response.Data!.ToString());

        List<SelectListItem> companyList = new List<SelectListItem>{
        new SelectListItem(){Text="All Company",Value=""}
        };
        List<SelectListItem> techStackList = new List<SelectListItem>{
        new SelectListItem(){Text="All Tech Stack",Value=""}
        };

        foreach(string? company in companyAndTechStackDict["companyNames"]){
            if(string.IsNullOrEmpty(company)){
                continue;
            }
            companyList.Add(new SelectListItem(){Text=company,Value=company});
        }

        foreach(string? techStack in companyAndTechStackDict["techStacks"]){
            if(string.IsNullOrEmpty(techStack)){
                continue;
            }
            techStackList.Add(new SelectListItem(){Text=techStack,Value=techStack});
        }

        companyAndTechStack.Add("companyNames",companyList);
        companyAndTechStack.Add("techStacks",techStackList);

        return  companyAndTechStack;
       }
   
    }
}
