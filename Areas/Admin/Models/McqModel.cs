using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    [TopicOrSubTopicRequired(ErrorMessage = "Please select either a  Topic or a Sub-topic, but not both.")]
    public class McqModel
    {
        // Primary Key
        [Key]
        public int McqId { get; set; }

        // Foreign Key to SubTopic
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Sub-topic.")]
        public int? SubTopicId { get; set; }

        // Navigation property for related SubTopic (optional, if you have a SubTopic model)
        [ForeignKey("SubTopicId")]
        public virtual SubTopicModel? SubTopic { get; set; }

        // Foreign Key to Topic
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid topic.")]
        public int? TopicId { get; set; }

        // Navigation property for related SubTopic (optional, if you have a SubTopic model)
        [ForeignKey("TopicId")]
        public virtual TopicModel? Topic { get; set; }

        // The actual question text
        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(500, ErrorMessage = "Question text can't be longer than 500 characters.")]
        public string QuestionText { get; set; }

        // Option A for the question
        [Required(ErrorMessage = "Option A is required.")]
        [StringLength(300, ErrorMessage = "Option A can't be longer than 300 characters.")]
        public string OptionA { get; set; }

        // Option B for the question
        [Required(ErrorMessage = "Option B is required.")]
        [StringLength(300, ErrorMessage = "Option B can't be longer than 300 characters.")]
        public string OptionB { get; set; }

        // Option C for the question
        [Required(ErrorMessage = "Option C is required.")]
        [StringLength(300, ErrorMessage = "Option C can't be longer than 300 characters.")]
        public string OptionC { get; set; }

        // Option D for the question
        [Required(ErrorMessage = "Option D is required.")]
        [StringLength(300, ErrorMessage = "Option D can't be longer than 300 characters.")]
        public string OptionD { get; set; }

        // The correct answer should be one of the options (A, B, C, D)
        [Required(ErrorMessage = "Correct Answer is required.")]
        [RegularExpression("^[A-D]$", ErrorMessage = "Correct Answer must be one of the options: A, B, C, or D.")]
        public string CorrectAnswer { get; set; }
    }
}



