using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    // [TopicOrSubTopicRequired(ErrorMessage = "Please select either a  Topic or a Sub-topic, but not both.")]
    public class QuestionModel
    {
        // Primary Key
        [Key]
        public Guid? QuestionId { get; set; }

        // Foreign Key to SubTopic
        public Guid? SubTopicId { get; set; }

        [ForeignKey("SubTopicId")]
        public virtual SubTopicModel? SubTopic { get; set; }

        // Foreign Key to Topic
        [Required(ErrorMessage = "Topic is required.")]
        public Guid TopicId { get; set; }

        [ForeignKey("TopicId")]
        public virtual TopicModel? Topic { get; set; }

        [Required(ErrorMessage = "Question  is required.")]
        [StringLength(500, ErrorMessage = "Question text can't be longer than 500 characters.")]
        public required string Question { get; set; }

        [Required(ErrorMessage = "Question Answer is required.")]
        [StringLength(1000, ErrorMessage = "Question Answer can't be longer than 1000 characters.")]
        [Display(Name = "Question Answer")]
        public required string QuestionAnswer { get; set; }
    }
}


public class TopicOrSubTopicRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Get the TopicId and SubTopicId properties from the validation context
        var topicIdProperty = validationContext.ObjectType.GetProperty("TopicId");
        var subTopicIdProperty = validationContext.ObjectType.GetProperty("SubTopicId");

        if (topicIdProperty == null || subTopicIdProperty == null )
        {
            return new ValidationResult("TopicId or SubTopicId fields are Required.");
        }

        var topicIdValue = topicIdProperty.GetValue(validationContext.ObjectInstance) as Guid?;
        var subTopicIdValue = subTopicIdProperty.GetValue(validationContext.ObjectInstance) as Guid?;

        if(topicIdValue == null && subTopicIdValue == null)
        {
            return new ValidationResult("TopicId or SubTopicId fields are Required.");
        }

        // Check if exactly one is provided (not both and not none)
        if ((topicIdValue.HasValue && !subTopicIdValue.HasValue) ||
            (!topicIdValue.HasValue && subTopicIdValue.HasValue))
        {
            return ValidationResult.Success;
        }

        // Validation failed, return error message
        return new ValidationResult("Please select either a  Topic or a  Sub-topic, but not both.");
    }
}