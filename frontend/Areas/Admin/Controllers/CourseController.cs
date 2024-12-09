using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Services;
using Placement_Preparation.Utils;
using System.Reflection;
using System.Linq;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly string _apiBaseUrl = "Course";
        private readonly ApiClientService _apiClient;
        private readonly AllDropDown _allDropDown;
        private readonly ExportService _exportService;

        public CourseController(ApiClientService apiClient , AllDropDown allDropDown , ExportService exportService)
        {
            _apiClient = apiClient;
            _allDropDown = allDropDown;
            _exportService = exportService;
        }
        
        #region list of Course
        public async Task<IActionResult> ListCourse()
        {
            ApiResponseModel response = await _apiClient.GetAsync(_apiBaseUrl);
                if(response.StatusCode != 200)
                {
                   TempData["ErrorMessage"] = response.Message;
                }
                List<CourseModel>  courseList =  JsonConvert.DeserializeObject<List<CourseModel>>(response.Data!.ToString());

           
            return View(courseList);
        }
        #endregion

        #region add or Edit Course
        public async Task<IActionResult> AddOrEditCourse(string? courseId)
        {
             // set drop down value
           await setDropDownsValue();

           
              // If courseId is null then it is add courseT
            if(courseId == null)
            {
                return View();
            }
           
             // Call API to get data
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{courseId}");
            if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("ListCourse");
                }

            // Convert response data to CourseTypeModel
            CourseModel course = JsonConvert.DeserializeObject<CourseModel>(response.Data!.ToString());
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditCourse(CourseModel course , IFormFile img)
        {
            // 
              if (course.CourseId != null)
            {
                // remove img validation error
                ModelState.Remove("Img");
            }


              // Check if image is not null
                if(img != null)
                {
                    // Validate image size (3MB = 3 * 1024 * 1024 bytes)
                    if (img.Length > 3 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Img", "Image size must be less than 3MB.");
                    }else{
                       // Call method to upload image
                        ApiResponseModel apiResponse = await _apiClient.UploadImage(img);
                        if(apiResponse.StatusCode == 200)
                        {
                            course.Img = apiResponse.Data!.ToString();
                            
                            // remove img validation error
                            ModelState.Remove("Img");
                        }
                        else
                        {
                            ModelState.AddModelError("Img", "Failed to upload image. Please try again.");
                        }
                    }

                    
                }else if (course.Img == null && course.CourseId == null)
                 {
                     ModelState.AddModelError("Img", "Image is required.");
                 }

              


            //Server side validation
            if (ModelState.IsValid)
            {
               ApiResponseModel response = new ApiResponseModel();

                if(course.CourseId != Guid.Empty && course.CourseId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, course);
                }else{
                     // Call API to save data
                     course.CourseId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, course);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("ListCourse");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }

            // set drop down value
            await setDropDownsValue();
            return View(course);
        }
        #endregion

        #region set Course Type and Branch DropDown Value
        [NonAction]
        public async Task setDropDownsValue()
        {
            // var allDropDown = new AllDropDown(_apiClient);
            ViewBag.CourseTypeList = await  _allDropDown.CourseType();
            ViewBag.BranchList =await _allDropDown.Branch();
        }
        #endregion

        
        #region delete Course
        [Route("/DeleteCourse/{courseId}")]
        public async Task<IActionResult> DeleteCourse(string courseId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{courseId}");
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListCourse");
        }
        #endregion

         #region  Delete Multiple  Courses
        public async Task<IActionResult> DeleteMultipleCourse(string  courseIds)
        {
            ApiResponseModel response = await _apiClient.DeleteMultipleAsync($"{_apiBaseUrl}/DeleteMultiple",courseIds.Split(","));
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListCourse");
        }
        #endregion

        #region Export Course to Excel
        public async Task<IActionResult> ExportToExcelCourse()
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}");
            if(response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ListCourse");
            }

            try
            {
                // Ensure response.Data is cast to IEnumerable<CourseModel>
                List<CourseModel> courseList = JsonConvert.DeserializeObject<List<CourseModel>>(response.Data!.ToString());

                //List<string> columns = GetAllColumnNames(typeof(CourseModel));
                List<string> columns = ["courseId" , "courseName" , "description" , "img", "branchId" , "branchName" , "courseTypeId" , "courseTypeName"];

                List<Dictionary<string,dynamic>> courseObeData = new List<Dictionary<string,dynamic>> { };

                foreach(CourseModel course in courseList)
                {
                    // Create a flattened dictionary
                    var flattenedObject = new Dictionary<string, dynamic>
                    {
                        { "courseId", course.CourseId.ToString() ?? "N/A" },
                        { "courseName", course.CourseName },
                        { "branchId", course.BranchId },
                        { "branchName", course.Branch?.BranchName ?? "N/A" }, // Extract branchName from branch
                        { "description", course.Description },
                        { "img", course.Img ?? "N/A"},
                        { "courseTypeId", course.CourseTypeId },
                        { "courseTypeName", course.CourseType?.CourseTypeName ?? "N/A" } // Extract courseTypeName from courseType
                    };
                    courseObeData.Add(flattenedObject);
                }

                // Call ExportToExcelBySpecificColumn method
                byte[] fileContents = _exportService.ExportToExcelBySpecificColumn(courseObeData, columns.ToArray());

                // Return as a file to trigger download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Course.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListCourse");
            }
        }

        #endregion
    }
}
