using System.ComponentModel.DataAnnotations;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class CourseTypeModel
    {
        [Key]
        public int CourseTypeId { get; set; }

        [Required(ErrorMessage = "Course Type  is required.")]
        [StringLength(100, ErrorMessage = "CourseType can't be longer than 100 characters.")]

        public int CourseTypeName { get; set; }
    }
}
