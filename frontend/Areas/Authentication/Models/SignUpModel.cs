using System.ComponentModel.DataAnnotations;

namespace Frontend.Areas.Authentication.Models
{
   public class SignUpModel{
        [Required]
        public string Name { get; set; }
        
        [Display(Name = "College Name")]
        public string? CollegeName {get;set;}

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        // password command validation like length , special character, number, upper case
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be 8-15 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character.")]
        public required string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public required string ConfirmPassword { get; set; }

    }

    public class EmailVarificationModel{
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }

    public class OTPModel
    {
        public  string? Email { get; set; }

        public  int? FirstDigit { get; set; }
        public  int? SecoundDigit { get; set; }
        public  int? ThirdDigit { get; set; }
        public  int? FourthDigit { get; set; }
        public  int? FifthDigit { get; set; }
        public  int? SixthDigit { get; set; }
    }
}