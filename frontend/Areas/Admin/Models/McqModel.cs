using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    // [TopicOrSubTopicRequired(ErrorMessage = "Please select either a  Topic or a Sub-topic, but not both.")]
    public class McqModel
    {
        // Primary Key
        [Key]
        public Guid? McqId { get; set; }

        // question text
        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(500, ErrorMessage = "Question text can't be longer than 500 characters.")]
        [Display(Name = "Question Text")]
        public required string QuestionText { get; set; }

        // Option A for the question
        [Required(ErrorMessage = "Option A is required.")]
        [StringLength(300, ErrorMessage = "Option A can't be longer than 300 characters.")]
        [Display(Name = "Option A")]
        public required string OptionA { get; set; }

        // Option B for the question
        [Required(ErrorMessage = "Option B is required.")]
        [StringLength(300, ErrorMessage = "Option B can't be longer than 300 characters.")]
        [Display(Name = "Option B")]
        public required string OptionB { get; set; }

        // Option C for the question
        [Required(ErrorMessage = "Option C is required.")]
        [StringLength(300, ErrorMessage = "Option C can't be longer than 300 characters.")]
        [Display(Name = "Option C")]
        public required string OptionC { get; set; }

        // Option D for the question
        [Required(ErrorMessage = "Option D is required.")]
        [StringLength(300, ErrorMessage = "Option D can't be longer than 300 characters.")]
        [Display(Name = "Option D")]
        public required string OptionD { get; set; }

         // The correct answer should be one of the options (A, B, C, D)
        [Required(ErrorMessage = "Correct Answer is required.")]
        [RegularExpression("^[A-D]$", ErrorMessage = "Correct Answer must be one of the options: A, B, C, or D.")]
        [Display(Name = "Correct Answer")]
        public required string CorrectAnswer { get; set; }

        //AnswerDescription
        [Required(ErrorMessage = "Answer Description is required.")]
        [Display(Name = "Answer Description")]
        public required string AnswerDescription { get; set; }

        // IsActive
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        
        public Guid? AddedBy { get; set; }

        public string? TechStack { get; set; }  

        public string? CompanyName { get; set; }

        public string? ApproveStatus { get; set; } = "Pending";

        // Foreign Key to Course
        public Guid? CourseId { get; set; }
        
        [ForeignKey("CourseId")]
        public CourseModel? Course { get; set; }

        // Foreign Key to Topic
        public Guid? TopicId { get; set; }

        [ForeignKey("TopicId")]
        public TopicModel? Topic { get; set; }

        // Foreign Key to SubTopic
        public Guid? SubTopicId { get; set; }

        [ForeignKey("SubTopicId")] 
        public SubTopicModel? SubTopic { get; set; }

        // Foreign Key to DefaultDifficultyLevel
        public Guid? DifficultyLevelId { get; set; }

        [ForeignKey("DifficultyLevelId")]
        public DifficultyLevelModel? DifficultyLevel { get; set; }

    }
}



