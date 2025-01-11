using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class BranchModel
    {
        // Primary Key
        [Key]
        public Guid BranchId { get; set; }

        public required string BranchName { get; set; }
    }
}
