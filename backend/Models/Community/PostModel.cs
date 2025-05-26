using System.ComponentModel.DataAnnotations;

namespace backend.Models
{ 
  public  class PostModel
    {
        [Key]
        public Guid? QuestionId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, ErrorMessage = "Title can't be longer than 150 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public required string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }
    }
}