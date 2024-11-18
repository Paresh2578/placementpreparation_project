using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Constant;

namespace backend.Models
{
    public class TopicModel
    {
        // Primary Key
        [Key]
        public Guid TopicId { get; set; }

        [Required(ErrorMessage = "Topic Name is required.")]
        [StringLength(100, ErrorMessage = "Topic Name can't be longer than 100 characters.")]
        public required string TopicName { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        [NotEmptyGuid(ErrorMessage = "InValid Course Id.")]
        public Guid CourseId { get; set; }

        [StringLength(5000, ErrorMessage = "Content can't be longer than 5000 characters.")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Difficulty Level is required.")]
        [NotEmptyGuid(ErrorMessage = "InValid Difficulty Level Id.")]
        public Guid DifficultyLevelId { get; set; }

    }
}
