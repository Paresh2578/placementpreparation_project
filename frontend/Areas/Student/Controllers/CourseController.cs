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
    public class CourseController : Controller
    {
        private readonly ApiClientService _apiClient;
        private readonly string _apiBaseUrl = "Course";
        private readonly AllDropDown _allDropDown;

        public CourseController(ApiClientService apiClient,AllDropDown allDropDown)
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
        }

        #region List All Courses
        public async Task<IActionResult> ListAllCourse([FromQuery] string? courseType)
        {
            
            // get top 5 courses
            ApiResponseModel responseModel = await _apiClient.GetAsync($"{_apiBaseUrl}?courseType={courseType}");

            List<CourseModel> courses = new List<CourseModel>();
            if (responseModel.StatusCode != 200)
            {
              ViewData["InternalServerError"] = responseModel.Message;
              return View(courses);
            }
              // set DropDown Value
               await SetBranchCourseDifficultyLevelDropdownValue();
                courses = JsonConvert.DeserializeObject<List<CourseModel>>(responseModel.Data.ToString());

            return View(courses);
        }
        #endregion

        #region Course Details
        public async Task<IActionResult> CourseDetail(string courseId)
        {
          ApiResponseModel apiResponse = await _apiClient.GetAsync($"{_apiBaseUrl}/GetCourseDetailsById/{courseId}");
          Dictionary<string,dynamic> courseDetails = new Dictionary<string, dynamic>();

          if(apiResponse.StatusCode != 200){
            TempData["ErrorMessage"] = apiResponse.Message;
            ViewData["InternalServerError"] =apiResponse.Message;
             return View(courseDetails);
          }

          courseDetails = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(apiResponse.Data.ToString());

          // set difficulty level dropdown
          ViewBag.DifficultyLevelList =await _allDropDown.DifficultyLevel();

            return View(courseDetails);
        }
        #endregion

        #region CourseRead
        public async Task<IActionResult> CourseRead(string courseId,string topicId , string? subTopicId)
        {
            // call api to fetch sidebar with documention
            ApiResponseModel apiResponse = await _apiClient.GetAsync($"Topic/GetSidebarDataByCourseIdAndTopicDocumentionByTopicId/{courseId}/{topicId}/{subTopicId}");
            Dictionary<string,dynamic> sidebarData = new Dictionary<string, dynamic>();

            // check successfully fetch data or not
            if(apiResponse.StatusCode != 200){
              TempData["ErrorMessage"] = apiResponse.Message;
              ViewData["InternalServerError"] =apiResponse.Message;
              return View(sidebarData);
            }

            sidebarData = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(apiResponse.Data!.ToString());

            return View(sidebarData);
        }
        #endregion
    
       #region  Set Branchs , Course , DifficultyLevel dropdown list
       [NonAction]
       public async Task SetBranchCourseDifficultyLevelDropdownValue(){
         ViewBag.BranchList =await  _allDropDown.Branch();
         ViewBag.CourseTypeList = await _allDropDown.CourseType();
         ViewBag.DifficultyLevelList =await _allDropDown.DifficultyLevel();
       }
       #endregion 

       #region Get Courses By Branch And Course Type
       [Route("/Course/GetCoursesByBranchAndCourseType")]
         public async Task<List<CourseModel>> GetCoursesByBranchAndCourseType([FromQuery] Guid? branchId,[FromQuery] Guid? courseTypeId)
         {
              ApiResponseModel responseModel = await _apiClient.GetAsync($"{_apiBaseUrl}/GetCoursesByBranchAndCourseType?branchId={branchId}&courseTypeId={courseTypeId}");
              List<CourseModel> courses = new List<CourseModel>();
              if (responseModel.StatusCode == 200)
              {
                courses = JsonConvert.DeserializeObject<List<CourseModel>>(responseModel.Data.ToString());
              }
              return courses;
         }
         #endregion
    }
}
