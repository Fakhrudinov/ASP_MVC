using System.ComponentModel.DataAnnotations;

namespace MVC_Messenger.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
