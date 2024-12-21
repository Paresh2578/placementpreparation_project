using Frontend.Models;
using Frontend.Services;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;

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
        public async Task<IActionResult> QuestionLists(string courseName , string courseId){
          ApiResponseModel apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}?courseId={courseId}&onlyActiveQuestions=true");

          List<QuestionModel> questions = new List<QuestionModel>();

          // check api response is success or not
          if(apiResponse.StatusCode != 200){
            TempData["ErrorMessage"] = apiResponse.Message;
            return View(questions);
          }

          // convert api response data to list of QuestionModel
          questions = JsonConvert.DeserializeObject<List<QuestionModel>>(apiResponse.Data.ToString());
          
          return View(questions);
        }
        #endregion

       }
}
