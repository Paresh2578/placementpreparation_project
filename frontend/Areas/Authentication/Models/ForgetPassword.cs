using System.ComponentModel.DataAnnotations;

namespace Frontend.Areas.Authentication.Models
{
    public class ForgetPassword
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public required string Email { get; set; }
    }
}