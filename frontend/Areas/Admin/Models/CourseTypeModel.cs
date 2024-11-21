using System.ComponentModel.DataAnnotations;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class CourseTypeModel
    {
        [Key]
        public Guid? CourseTypeId { get; set; } = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [Required(ErrorMessage = "Course Type  is required.")]
        [StringLength(100, ErrorMessage = "CourseType can't be longer than 100 characters.")]
        public required string CourseTypeName { get; set; }
    }
}
