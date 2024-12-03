using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class CourseModel
    {
        // Primary Key
        [Key]
        public Guid? CourseId { get; set; }

        // course Name
        [Required(ErrorMessage = "Course Name is required.")]
        [StringLength(100, ErrorMessage = "Course Name can't be longer than 100 characters.")]
        [Display(Name = "Course Name")]
        public required string CourseName { get; set; }

        // Description
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters.")]
        public required string Description { get; set; }

        // Img
        // [Required(ErrorMessage = "")]
        // [Url(ErrorMessage = "Please provide a valid URL for the image.")]
        public string? Img { get; set; }

        // ForeigKey to Branch
        [Required(ErrorMessage = "Branch is required.")]
        [Display(Name = "Branch")]
        public Guid BranchId { get; set; }

        [ForeignKey("BranchId")]
        public  BranchModel? Branch { get; set; }

        
        // ForeignKey to Course Type
        [Required(ErrorMessage = "Course Type is required.")]
        [Display(Name = "Course Type")]
        public Guid CourseTypeId { get; set; }

        [ForeignKey("CourseTypeId")]
        public CourseTypeModel? CourseType { get; set; }
    }
}
