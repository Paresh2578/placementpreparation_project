using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
   public class QuestionAnswerModel
    {
        [Key]
        public Guid QuestionAnswerId { get; set; }

        [Required(ErrorMessage = "Answer is required.")]
        public required string Answer { get; set; }
        
        [Required(ErrorMessage = "Question ID is required.")]
        public required Guid QuestionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public required Guid UserId { get; set; }
    }
}