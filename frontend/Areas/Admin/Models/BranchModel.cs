using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using backend.Constant;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class BranchModel
    {
        // Primary Key
        // [NotEmptyGuid(ErrorMessage = "Branch Id is required.")]
        public  Guid? BranchId { get; set; } = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [Required(ErrorMessage = "Branch Name is required.")]
        [StringLength(100, ErrorMessage = "Branch Name can't be longer than 100 characters.")]
        public required string BranchName { get; set; }
    }
}
