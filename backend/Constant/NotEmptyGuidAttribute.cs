using System.ComponentModel.DataAnnotations;

namespace backend.Constant
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Guid guid && guid == Guid.Empty)
            {
                return new ValidationResult(ErrorMessage ?? "The Guid cannot be empty.");
            }

            return ValidationResult.Success;
        }
    }
}
