using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class QuestionModel
    {
        // Primary Key
        [Key]
        public int QuestionId { get; set; }

        // Foreign Key to SubTopic
        [Required(ErrorMessage = "Sub-topic is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Sub-topic.")]
        public int SubTopicId { get; set; }

        [ForeignKey("SubTopicId")]
        public virtual SubTopicModel SubTopic { get; set; }

        // The actual question with validation
        [Required(ErrorMessage = "Question  is required.")]
        [StringLength(500, ErrorMessage = "Question text can't be longer than 500 characters.")]
        public string Question { get; set; }

        // The answer to the question with validation
        [Required(ErrorMessage = "Question Answer is required.")]
        [StringLength(1000, ErrorMessage = "Question Answer can't be longer than 1000 characters.")]
        public string QuestionAnswer { get; set; }
    }
}
