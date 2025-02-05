using System.ComponentModel.DataAnnotations;

namespace Placement_Preparation.Areas.Admin.Models{
    public class FeedbackModel{
        [Key]
        public Guid FeedbackId { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string FeedbackType { get; set; }
        public required string FeedbackMessage { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
    }
}