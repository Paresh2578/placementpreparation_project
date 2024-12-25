using Frontend.Models;
using Frontend.Services;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;
using X.PagedList.Extensions;

namespace Placement_Preparation.Areas.Student.Controllers
{
    [Area("Student")]
    public class McqController : Controller
    {
        private readonly ApiClientService _apiClient;
        private readonly string _apiBaseUrl = "Mcq";
        private readonly AllDropDown _allDropDown;

        public McqController(ApiClientService apiClient,AllDropDown allDropDown)
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
        }

        #region List All Courses which are avalible in mcq
        public async Task<IActionResult> ListAllCourses(){
          ApiResponseModel apiResponse = await _apiClient.GetAsync("Course/GetCoursesNameListThatMcqIsAvalible");

          List<Dictionary<string,dynamic>> courseData = new List<Dictionary<string, dynamic>>();

          // check api response is success or not
          if(apiResponse.StatusCode != 200){
            TempData["ErrorMessage"] = apiResponse.Message;
            return View(courseData);
          }

          // convert api response data to list of dictionary
          courseData = JsonConvert.DeserializeObject<List<Dictionary<string,dynamic>>>(apiResponse.Data.ToString());
          
          return View(courseData);
        }
        #endregion

        #region  List All Mcq by courses 
        public async Task<IActionResult> McqLists(string courseName , string courseId,int? pageNumber=1,int? pageSize=5){
          ApiResponseModel apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}?courseId={courseId}&onlyActiveMcqs=true&pageNumber={pageNumber}&pageSize={pageSize}");

          Dictionary<string,dynamic> mcqData = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(apiResponse.Data.ToString());
          List<McqModel> mcqs = new List<McqModel>();

          // check api response is success or not
          if(apiResponse.StatusCode != 200){
            TempData["ErrorMessage"] = apiResponse.Message;
            return View(mcqs);
          }

          // add empty data for pagination
          mcqs.AddRange(Enumerable.Repeat(new McqModel 
          { 
              QuestionText = string.Empty, 
              OptionA = string.Empty, 
              OptionB = string.Empty, 
              OptionC = string.Empty, 
              OptionD = string.Empty, 
              CorrectAnswer = string.Empty, 
              AnswerDescription = string.Empty 
          }, Convert.ToInt32(mcqData["totalMcqs"])));

          // fill current page data only. Other data will be empty
          int j=0;
          for(int i=(pageNumber-1)*pageSize??1;i<=(pageNumber*pageSize) && j < mcqData["mcqs"].Count;i++){
            mcqs[i] = JsonConvert.DeserializeObject<McqModel>(mcqData["mcqs"][j].ToString());
            j++;
          }

          // convert list of McqModel to list of paged McqModel
          var pagedMcqs = mcqs.ToPagedList(pageNumber ?? 1, pageSize??1);

          // convert api response data to list of McqModel
          // mcqs = JsonConvert.DeserializeObject<List<McqModel>>(apiResponse.Data.ToString());
          
          return View(pagedMcqs);
        }
        #endregion

       }
}
