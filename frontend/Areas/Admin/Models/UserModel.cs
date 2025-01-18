using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class UserModel
    {
        [Key]
        public Guid AdminUserId { get; set; }

        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]

        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }

        public string? CollegeName { get; set; }

        public bool? IsAdmin { get; set; } = false;
        public string? ApproveStatus { get; set; } = "Pending";
    }
}