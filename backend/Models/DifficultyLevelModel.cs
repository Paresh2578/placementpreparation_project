using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    // Enum to define the difficulty levels
    public enum DifficultyLevelName
    {
        Easy,
        Medium,
        Hard
    }

    public class DifficultyLevelModel
    {
        // Primary Key
        [Key]
        public Guid DifficultyLevelId { get; set; }

        // Name of the Difficulty Level (e.g., Easy, Medium, Hard)
        [Required(ErrorMessage = "Difficulty Level is required.")]
        [StringLength(100, ErrorMessage = "Difficulty Level can't be longer than 100 characters.")]
        public required string DifficultyLevelName { get; set; }
    }
}
