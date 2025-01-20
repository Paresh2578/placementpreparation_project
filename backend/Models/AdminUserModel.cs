using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class AdminUserModel
    {
        [Key]
        public Guid AdminUserId { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]

        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }

        public string? CollegeName { get; set; }

        public bool? IsAdmin { get; set; } = false;
        public string? ApproveStatus { get; set; } = "Pending";

        public string? Token { get; set; }
        public DateTime? TokenExpiryTime { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "New Password is required.")]
         [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be 8-15 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character.")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Token is required.")]
        public required string Token { get; set; }
    }
}
