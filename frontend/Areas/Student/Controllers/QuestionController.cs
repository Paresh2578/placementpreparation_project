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
    public class QuestionController : Controller  
    {
        private readonly ApiClientService _apiClient;
        private readonly string _apiBaseUrl = "Question";
        private readonly AllDropDown _allDropDown;

        public QuestionController(ApiClientService apiClient,AllDropDown allDropDown)
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
        }

        #region List All Courses which are avalible in Question
        public async Task<IActionResult> ListAllCourses(){
          ApiResponseModel apiResponse = await _apiClient.GetAsync("Course/GetCoursesNameListThatQuestionIsAvalible");

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

        #region  List All Question by courses 
        public async Task<IActionResult> QuestionList(string courseName , string courseId,int? pageNumber,int? pageSize=5){
          // ApiResponseModel apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}?courseId={courseId}&onlyActiveQuestions=true&pageNumber={pageNumber}&pageSize={pageSize}");
          ApiResponseModel apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}?courseId={courseId}&onlyActiveQuestions=true&pageSize={pageSize}");
          // Dictionary<string,dynamic> questionData = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(apiResponse.Data.ToString());
          

          List<QuestionModel> questions = new List<QuestionModel>();

          // add empty data
          // questions.AddRange(Enumerable.Repeat(new QuestionModel { Question = string.Empty, QuestionAnswer = string.Empty }, questionData["totalQuestions"]));


          // check api response is success or not
          if(apiResponse.StatusCode != 200){
            TempData["ErrorMessage"] = apiResponse.Message;
            return View(questions);
          }

          // convert api response data to list of QuestionModel
          // questions = JsonConvert.DeserializeObject<List<QuestionModel>>(questionData["questions"].ToString());
          questions = JsonConvert.DeserializeObject<List<QuestionModel>>(apiResponse.Data.ToString());

          // convert list of QuestionModel to list of paged QuestionModel
          var pagedQuestions = questions.ToPagedList(pageNumber ?? 1, pageSize??1);
          
          return View(pagedQuestions);
        }
        #endregion

       }
}
