using Frontend.Models;
using Frontend.Services;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Areas.Student.Controllers
{
    [Area("Student")]
    public class CourseController : Controller
    {
        private readonly ApiClientService _apiClient;
        private readonly string _apiBaseUrl = "Course";

        public CourseController(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        #region List All Courses
        public async Task<IActionResult> ListAllCourse()
        {
            
            // get top 5 courses
            ApiResponseModel responseModel = await _apiClient.GetAsync($"{_apiBaseUrl}");

            List<CourseModel> courses = new List<CourseModel>();
            if (responseModel.StatusCode == 200)
            {
                courses = JsonConvert.DeserializeObject<List<CourseModel>>(responseModel.Data.ToString());
            }
            return View(courses);
        }
        #endregion

        #region Course Details
        public IActionResult CourseDetail()
        {
            return View();
        }
        #endregion

        #region CourseRead
        public IActionResult CourseRead()
        {
           
            string markdown = @"# Introduction to JavaScript

JavaScript is a versatile programming language that powers the modern web. It allows you to create interactive and dynamic websites.

## What is JavaScript?

JavaScript is a high-level, interpreted programming language that conforms to the ECMAScript specification. It was created by Brendan Eich in 1995 and has since become one of the world's most popular programming languages.

## Key Features

- **Dynamic Typing**: Variables can hold different types of values
- **First-class Functions**: Functions can be assigned to variables
- **Object-Oriented**: Supports object-oriented programming
- **Event-Driven**: Perfect for handling user interactions

## Getting Started

Here's a simple example:

```javascript
// Your first JavaScript code
console.log('Hello, World!');

// Variables
let name = 'John';
const age = 25;

// Functions
function greet(name) {
  return `Hello, ${name}!`;
}
```";

var pipeline = new MarkdownPipelineBuilder()
    .UseAdvancedExtensions() // Enables advanced Markdown features
    .Build();

            string htmlContent = Markdown.ToHtml(markdown , pipeline);
            ViewData["Content"] = htmlContent;
            return View();
        }
        #endregion
    }
}
