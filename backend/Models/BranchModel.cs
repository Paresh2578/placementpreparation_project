using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class BranchModel
    {
        // Primary Key
        [Key]
        public Guid BranchId { get; set; }

        [Required(ErrorMessage = "Branch Name is required")]
        public required string BranchName { get; set; }
    }
}
