using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Constant;

namespace backend.Models
{
    public class SubTopicModel
    {
        // Primary Key
        [Key]
        public Guid SubTopicId { get; set; }

        // Sub-topic Name with validation
        [Required(ErrorMessage = "Sub-topic Name is required.")]
        [StringLength(200, ErrorMessage = "Sub-topic Name can't be longer than 200 characters.")]
        public required string SubTopicName { get; set; }

        // Foreign Key to Topic
        [Required(ErrorMessage = "Topic is required.")]
        [NotEmptyGuid(ErrorMessage = "InValid Topic Id.")]
        public Guid TopicId { get; set; }

        [ForeignKey("TopicId")]
        public TopicModel? Topic { get; set; }

        // Content for the Sub-topic with validation
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(2000, ErrorMessage = "Content can't be longer than 2000 characters.")]
        public required string Content { get; set; }

       
    }
}
