using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Areas.Student.Controllers
{
    [Area("Student")]
    public class HomeController : Controller
    {
        private readonly ApiClientService _apiClient;

        public HomeController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<IActionResult> Home()
        {

            // get top 5 courses
            ApiResponseModel responseModel = await _apiClient.GetAsync("Course?limit=5");

            List<CourseModel> courses = new List<CourseModel>();
            if (responseModel.StatusCode == 200)
            {
                courses = JsonConvert.DeserializeObject<List<CourseModel>>(responseModel.Data.ToString());
            }else{
              ViewData["InternalServerError"] = responseModel.Message;
            }

            /// write a code for two sum
            
            


            return View(courses);
        }
    }
}
