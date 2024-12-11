using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using backend.Constant;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class BranchModel
    {
        // Primary Key
        // [NotEmptyGuid(ErrorMessage = "Branch Id is required.")]
        [Key]
        public  Guid? BranchId { get; set; }

        [Required(ErrorMessage = "Branch Name is required.")]
        [StringLength(100, ErrorMessage = "Branch Name can't be longer than 100 characters.")]
        [Display(Name = "Branch Name")]
        public required string BranchName { get; set; }
    }
}
