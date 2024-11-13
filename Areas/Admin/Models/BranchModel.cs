using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class BranchModel
    {
        // Primary Key
        [Key]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Branch Name is required.")]
        [StringLength(100, ErrorMessage = "Branch Name can't be longer than 100 characters.")]
        public string BranchName { get; set; }
    }
}
