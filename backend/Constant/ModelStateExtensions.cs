using Microsoft.AspNetCore.Mvc;

namespace backend.Constant
{
    public static class ModelStateExtensions
    {
        // Extension method to get validation errors
        public static IActionResult GetValidationErrors(this ControllerBase controller)
        {
            var errors = controller.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return new BadRequestObjectResult(new { Errors = errors });
        }

    }
}
