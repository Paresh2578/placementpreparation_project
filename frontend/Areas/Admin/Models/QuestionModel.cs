using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frontend.Areas.Authentication.Models;

namespace Placement_Preparation.Areas.Admin.Models
{
    // [TopicOrSubTopicRequired(ErrorMessage = "Please select either a  Topic or a Sub-topic, but not both.")]
    public class QuestionModel
    {
        // Primary Key
        [Key]
        public Guid? QuestionId { get; set; }

        // Question
        [Required(ErrorMessage = "Question  is required.")]
        public required string Question { get; set; }

        // Question Answer
        [Required(ErrorMessage = "Question Answer is required.")]
        [Display(Name = "Question Answer")]
        public required string QuestionAnswer { get; set; }

        // Is Active
        public bool IsActive { get; set; } = true;

        // Tech Stack Name
        [Display(Name = "Tech Stack")]
        public string? TechStack { get; set; }

        // Compnay Name
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        // Added By
        public Guid? AddedBy { get; set; }

        [ForeignKey("AddedBy")]
        public SignUpModel? AddedByAdminUser { get; set; }

        
         public string? ApproveStatus {get;set;} = "Pending";

         // Foreign Key to Course
        // [Required(ErrorMessage = "Course is required.")]
        public Guid? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual CourseModel? Course { get; set; }

        // Foreign Key to Topic
        // [Required(ErrorMessage = "Topic is required.")]
        public Guid? TopicId { get; set; }

        [ForeignKey("TopicId")]
        public virtual TopicModel? Topic { get; set; }


        // Foreign Key to SubTopic
        public Guid? SubTopicId { get; set; }

        [ForeignKey("SubTopicId")]
        public virtual SubTopicModel? SubTopic { get; set; }

        //Foreign Key to Difficulty Level
        // [Required(ErrorMessage = "Difficulty Level is required.")]
        public Guid? DifficultyLevelId { get; set; }

        [ForeignKey("DifficultyLevelId")]
        public virtual DifficultyLevelModel? DifficultyLevel { get; set; }
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