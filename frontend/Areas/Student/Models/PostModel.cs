using System.ComponentModel.DataAnnotations;

namespace Placement_Preparation.Areas.Student.Models
{
    public class PostModel
    {
        [Key]
        public Guid? QuestionId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, ErrorMessage = "Title can't be longer than 150 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public required string Description { get; set; }
        
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid? UserId { get; set; }
    }
}