﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Constant;

namespace backend.Models
{
    public class CourseModel
    {
        // Primary Key
        [Key]
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Course Name is required.")]
        [StringLength(100, ErrorMessage = "Course Name can't be longer than 100 characters.")]
        public required string CourseName { get; set; }

        [Required(ErrorMessage = "Branch is required.")]
        [NotEmptyGuid(ErrorMessage = "Please select a valid Branch.")]
        public Guid BranchId { get; set; }

        [ForeignKey("BranchId")]
        public BranchModel? Branch { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        [Url(ErrorMessage = "Please provide a valid URL for the image.")]
        public required string Img { get; set; }

        [Required(ErrorMessage = "Course Type is required.")]
        [NotEmptyGuid(ErrorMessage = "Please select a valid Course Type.")]
        public Guid CourseTypeId { get; set; }

        [ForeignKey("CourseTypeId")]
        public CourseTypeModel? CourseType { get; set; }
    }
}
