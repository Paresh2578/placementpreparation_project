using System.ComponentModel.DataAnnotations;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class BranchModel
    {
        // Primary Key
        [Key]
        public int BranchId { get; set; }

        public string BranchName { get; set; }
    }
}
