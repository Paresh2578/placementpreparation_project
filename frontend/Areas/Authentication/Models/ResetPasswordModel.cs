using System.ComponentModel.DataAnnotations;

namespace Frontend.Areas.Authentication.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be 8-15 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [Display(Name = "New Password")]
        public required string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password must match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        public required string Token { get; set; }
    }
}
