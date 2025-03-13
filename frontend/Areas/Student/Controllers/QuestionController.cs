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
            ViewData["InternalServerError"] = apiResponse.Message;
            return View(courseData);
          }

          // convert api response data to list of dictionary
          courseData = JsonConvert.DeserializeObject<List<Dictionary<string,dynamic>>>(apiResponse.Data.ToString());
          
          return View(courseData);
        }
        #endregion

        #region  List All Question by courses 
        public async Task<IActionResult> QuestionList(string courseName,string? companyName , string? techStack, string? courseId,int? pageNumber=1,int? pageSize=10){
          try{
              ApiResponseModel apiResponse = new ApiResponseModel();

              
            List<QuestionModel> questions = new List<QuestionModel>();

         

            if(courseId == null){
              // get interview  questions
              apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}/InterviewQuestions?onlyActiveQuestions=true&onlyAcceptApprovalStatus=true&pageNumber={pageNumber}&pageSize={pageSize}&techStack={techStack}&companyName={companyName}");

                    // check api response is success or not
                    if (apiResponse.StatusCode == 200)
                    {
                        // set compnay name and tech stack dropdown value
                        Dictionary<string, List<SelectListItem>> comanyNameAndTechStack = await _allDropDown.GetQuestionCompanyAndTechStack();

                        if (!comanyNameAndTechStack.ContainsKey("companyNames"))
                        {
                            ViewData["InternalServerError"] = "Some Error for fetching data";
							return View(questions.ToPagedList(pageNumber ?? 0, pageSize ?? 0));
                        }

                        ViewBag.companyNames = comanyNameAndTechStack["companyNames"];
                        ViewBag.techStacks = comanyNameAndTechStack["techStacks"];
                    }
            }else{
              // get couese wise questions
              apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}?courseId={courseId}&onlyActiveQuestions=true&pageNumber={pageNumber}&pageSize={pageSize}");
            }

				// check api response is success or not
				if (apiResponse.StatusCode != 200)
				{
					ViewData["ErrorMessage"] = apiResponse.Message;
					ViewData["InternalServerError"] = apiResponse.Message;
					return View(questions.ToPagedList(pageNumber ?? 0, pageSize ?? 0));
				}



				Dictionary<string, dynamic> questionData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(apiResponse.Data.ToString());

                // add empty data for pagination
                questions.AddRange(Enumerable.Repeat(new QuestionModel { Question = string.Empty, QuestionAnswer = string.Empty }, Convert.ToInt32(questionData["totalQuestions"])));

            // fill current page data only. Other data will be empty
            int j=0;
            for(int i=(pageNumber-1)*pageSize??1;i<=(pageNumber*pageSize) && j < questionData["questions"].Count;i++){
              questions[i] = JsonConvert.DeserializeObject<QuestionModel>(questionData["questions"][j].ToString());
              j++;
            }
          
            // convert list of QuestionModel to list of paged QuestionModel
            var pagedQuestions = questions.ToPagedList(pageNumber ?? 1, pageSize??1);
            
            return View(pagedQuestions);
          }catch(Exception ex){
            ViewData["ErrorMessage"] = ex.Message;
            ViewData["InternalServerError"] = ex.Message;
            return View(new List<QuestionModel>());
          }
        }
        #endregion

       }
}