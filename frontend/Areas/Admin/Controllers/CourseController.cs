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
            var allDropDown = new AllDropDown(_apiClient);
            ViewBag.CourseTypeList = await  allDropDown.CourseType();
            ViewBag.BranchList =await allDropDown.Branch();
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
    }
}
