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
        public required string SubTopicName { get; set; }

        // Foreign Key to Topic
        [Required(ErrorMessage = "Topic is required.")]
        public Guid TopicId { get; set; }

        // Navigation property for related Topic (optional, if you have a Topic model)
        [ForeignKey("TopicId")]
        public virtual TopicModel? Topic { get; set; }

        // Content for the Sub-topic with validation
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(2000, ErrorMessage = "Content can't be longer than 2000 characters.")]
        [MinLength(10, ErrorMessage = "Content must be at least 10 characters long.")]
        public required string Content { get; set; }

       
    }
}
