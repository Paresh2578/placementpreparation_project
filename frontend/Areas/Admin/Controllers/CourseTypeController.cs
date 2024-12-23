using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;
using Placement_Preparation.Services;
using Placement_Preparation.BAL;

namespace Placement_Preparation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckAccess]
    public class CourseTypeController : Controller
    {
        private readonly string _apiBaseUrl = "CourseType";
        private readonly ApiClientService _apiClient;
        public readonly ExportService _exportService;
        
        public CourseTypeController(ApiClientService apiClient , ExportService exportService)
        {
            _apiClient = apiClient;
            _exportService = exportService;
        }

        #region list of  CourseType 
        public async Task<IActionResult> ListCourseType()
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


        #region add or Edit Branch
        public async Task<IActionResult> AddOrEditCourseType(string? courseTypeId)
        {
              // If courseTypeId is null then it is add courseType
            if(courseTypeId == null)
            {
                return View();
            }
           
                // Call API to get data
                ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}/{courseTypeId}");
                if(response.StatusCode != 200)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("List");
                }
                CourseTypeModel courseType = JsonConvert.DeserializeObject<CourseTypeModel>(response.Data!.ToString());
                return View(courseType);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditCourseType(CourseTypeModel courseType)
        {
            //Server side validation
            if (ModelState.IsValid)
            {
                 ApiResponseModel response = new ApiResponseModel();
                if(courseType.CourseTypeId != Guid.Empty && courseType.CourseTypeId != null)
                {
                    // Call API to update data
                     response = await _apiClient.PutAsync(_apiBaseUrl, courseType);
                }else{
                     // Call API to save data
                     courseType.CourseTypeId = Guid.NewGuid();
                      response = await _apiClient.PostAsync(_apiBaseUrl, courseType);
                }
               
                if(response.StatusCode == 201 || response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("ListCourseType");
                }else {
                     TempData["ErrorMessage"] = response.Message;
                }
            }
            return View(courseType);
        }
        #endregion
    
        #region delete CourseType
        [Route("/DeleteCourseType/{courseTypeId}")]
        public async Task<IActionResult> DeleteCourseType(string courseTypeId)
        {
            ApiResponseModel response = await _apiClient.DeleteAsync($"{_apiBaseUrl}/{courseTypeId}");
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListCourseType");
        }
        #endregion
        

        #region  Delete Multiple  Courses Type
        public async Task<IActionResult> DeleteMultipleCourseType(string  courseTypeIds)
        {
            ApiResponseModel response = await _apiClient.DeleteMultipleAsync($"{_apiBaseUrl}/DeleteMultiple",courseTypeIds.Split(","));
            if(response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = response.Message;
            }else {
                TempData["ErrorMessage"] = response.Message;
            }
            return RedirectToAction("ListCourseType");
        }
        #endregion

         #region Export CourseType to Excel
        public async Task<IActionResult> ExportToExcelCourseType()
        {
            ApiResponseModel response = await _apiClient.GetAsync($"{_apiBaseUrl}");
            if(response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ListCourseType");
            }

            try
            {
                 // Ensure response.Data is cast to IEnumerable<CourseTypeModel>
                var courseTypeModelList = JsonConvert.DeserializeObject<List<CourseTypeModel>>(response.Data!.ToString()) as IEnumerable<CourseTypeModel>;
                byte[] fileContents = _exportService.ExportToExcel(courseTypeModelList, "CourseType");

                // Return as a file to trigger download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CourseType.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListCourseType");
            }
        }
        #endregion
    }
}
