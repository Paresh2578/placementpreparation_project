using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Utils;
using System.Reflection.Metadata.Ecma335;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly string _apiBaseUrl = "Course";
        private readonly ApiClientService _apiClient;

        public CourseController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }
        
        #region list of Course
        public async Task<IActionResult> ListCourse()
        {
            ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<CourseTypeModel>  courseTypeList =  JsonConvert.DeserializeObject<List<CourseTypeModel>>(response.Data!.ToString());

           
            return View(courseTypeList);
        }
        #endregion

        #region add or Edit Course
        public IActionResult AddOrEditCourse(int? courseId)
        {
            setDropDownsValue();

            return View();
        }

        [HttpPost]
        public IActionResult AddOrEditCourse(CourseModel course)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListCourse");
            }
            setDropDownsValue();
            return View(course);
        }
        #endregion

        #region set Course Type and Branch DropDown Value
        [NonAction]
        public void setDropDownsValue()
        {
            ViewBag.CourseTypeList = AllDropDown.CourseType();
            ViewBag.BranchList = AllDropDown.Branch();
        }
        #endregion
    }
}
