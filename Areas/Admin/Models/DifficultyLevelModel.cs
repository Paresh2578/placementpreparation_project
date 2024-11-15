using System.ComponentModel.DataAnnotations;

namespace Placement_Preparation.Areas.Admin.Models
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
        public int DifficultyLevelId { get; set; }

        // Name of the Difficulty Level (e.g., Easy, Medium, Hard)
        [Required(ErrorMessage = "Difficulty Level is required.")]
        
        public DifficultyLevelName DifficultyLevelName { get; set; }
    }
}
