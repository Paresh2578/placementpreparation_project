using Frontend.Models;
using Frontend.Services;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
             ViewData["InternalServerError"] = apiResponse.Message;
            return View(courseData);
          }

          // convert api response data to list of dictionary
          courseData = JsonConvert.DeserializeObject<List<Dictionary<string,dynamic>>>(apiResponse.Data.ToString());
          
          return View(courseData);
        }
        #endregion

      #region List All Mcq by courses
      public async Task<IActionResult> McqLists(string courseName,string? companyName , string? techStack, string? courseId, int? pageNumber = 1, int? pageSize = 10)
      {
          try
          {
              ApiResponseModel apiResponse = new ApiResponseModel();

              if (courseId == null)
              {
                  // Get interview MCQs
                  apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}/GetInterviewMcqs?onlyActiveMcqs=true&onlyAcceptApprovalStatus=true&pageNumber={pageNumber}&pageSize={pageSize}&techStack={techStack}&companyName={companyName}");

                  if(apiResponse.StatusCode == 200){
                    // set compnay name and tech stack dropdown value
                    Dictionary<string,List<SelectListItem>> comanyNameAndTechStack = await _allDropDown.GetMcqCompanyAndTechStack();

                    if(comanyNameAndTechStack.ContainsKey("companyNames") && comanyNameAndTechStack.ContainsKey("techStacks")){
                      ViewBag.companyNames = comanyNameAndTechStack["companyNames"];
                      ViewBag.techStacks = comanyNameAndTechStack["techStacks"];
                    }
                  }
              }
              else
              {
                  // Get course-wise MCQs
                  apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}?courseId={courseId}&onlyActiveMcqs=true&pageNumber={pageNumber}&pageSize={pageSize}");
              }

              if (apiResponse.StatusCode != 200)
              {
                  TempData["ErrorMessage"] = apiResponse.Message;
                  ViewData["InternalServerError"] = apiResponse.Message;
                  return View(Enumerable.Empty<McqModel>().ToPagedList(pageNumber ?? 1, pageSize ?? 10));
              }

              Dictionary<string, dynamic> mcqData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(apiResponse.Data.ToString());

              int totalMcqs = Convert.ToInt32(mcqData["totalMcqs"]);
              List<McqModel> mcqs = Enumerable.Repeat(new McqModel
              {
                  QuestionText = string.Empty,
                  OptionA = string.Empty,
                  OptionB = string.Empty,
                  OptionC = string.Empty,
                  OptionD = string.Empty,
                  CorrectAnswer = string.Empty,
                  AnswerDescription = string.Empty
              }, totalMcqs).ToList();

              int startIndex = ((pageNumber ?? 1) - 1) * (pageSize ?? 10);
              int endIndex = Math.Min(startIndex + (pageSize ?? 10), totalMcqs);

              int j = 0;
              for (int i = startIndex; i < endIndex && j < mcqData["mcqs"].Count; i++, j++)
              {
                  mcqs[i] = JsonConvert.DeserializeObject<McqModel>(mcqData["mcqs"][j].ToString());
              }

              var pagedMcqs = mcqs.ToPagedList(pageNumber ?? 1, pageSize ?? 10);
              return View(pagedMcqs);
          }
          catch (Exception ex)
          {
              TempData["ErrorMessage"] = ex.Message;
                ViewData["InternalServerError"] = ex.Message;
              // Return an empty paged list in case of an exception
              return View(Enumerable.Empty<McqModel>().ToPagedList(pageNumber ?? 1, pageSize ?? 10));
          }
      }
      #endregion
       }
}
