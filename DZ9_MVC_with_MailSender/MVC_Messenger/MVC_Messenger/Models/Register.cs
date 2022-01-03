using System.ComponentModel.DataAnnotations;

namespace MVC_Messenger.Models
{
    public class Register
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Name or Alias: ")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be entered")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Incorrect password confirmation!")]
        public string ConfirmPassword { get; set; }

        [Phone]
        [Display(Name = "phone number: ")]
        public string PhoneNumber { get; set; }
    }
}
