using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Student.Controllers
{
    [Area("Student")]
    public class CourseController : Controller
    {
        #region List All Courses
        public IActionResult ListAllCourse()
        {
            return View();
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
