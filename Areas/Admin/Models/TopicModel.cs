using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class TopicModel
    {
        // Primary Key
        [Key]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Topic Name is required.")]
        [StringLength(100, ErrorMessage = "Topic Name can't be longer than 100 characters.")]
        public string TopicName { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Course.")]
        public int? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(5000, ErrorMessage = "Content can't be longer than 5000 characters.")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Difficulty Level is required.")]
        public int DifficultyLevelId { get; set; }

        [ForeignKey("DifficultyLevelId")]
        public DifficultyLevelModel DifficultyLevel { get; set; }
    }
}
