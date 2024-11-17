using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class CourseTypeModel
    {
        [Key]
        public Guid CourseTypeId { get; set; }

        [Required(ErrorMessage = "Course Type  is required.")]
        [StringLength(100, ErrorMessage = "CourseType can't be longer than 100 characters.")]
        public required string CourseTypeName { get; set; }
    }
}
