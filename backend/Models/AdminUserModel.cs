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
    }
}
