using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class SubTopicModel
    {
        // Primary Key
        [Key]
        public Guid? SubTopicId { get; set; }

        // Sub-topic Name with validation
        [Required(ErrorMessage = "Sub-topic Name is required.")]
        [StringLength(200, ErrorMessage = "Sub-topic Name can't be longer than 200 characters.")]
        [Display(Name = "Sub-topic Name")]
        public required string SubTopicName { get; set; }

         // Foreign Key to Course
        [Required(ErrorMessage = "Course is required.")]
        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual CourseModel? Course { get; set; }

        // Foreign Key to Topic
        [Required(ErrorMessage = "Topic is required.")]
        public Guid TopicId { get; set; }

        [ForeignKey("TopicId")]
        public virtual TopicModel? Topic { get; set; }

        // Foreign Key to Difficulty Level
        [Required(ErrorMessage = "Difficulty Level is required.")]
        public Guid DifficultyLevelId { get; set; }

        [ForeignKey("DifficultyLevelId")]
        public virtual DifficultyLevelModel? DifficultyLevel { get; set; }

        // Content for the Sub-topic with validation
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(2000, ErrorMessage = "Content can't be longer than 2000 characters.")]
        [MinLength(10, ErrorMessage = "Content must be at least 10 characters long.")]
        public required string Content { get; set; }

       
    }
}
