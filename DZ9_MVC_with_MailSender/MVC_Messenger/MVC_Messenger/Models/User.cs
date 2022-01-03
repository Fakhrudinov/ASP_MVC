using System.ComponentModel.DataAnnotations;

namespace MVC_Messenger.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Name or Alias: ")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "phone number: ")]
        public string PhoneNumber { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
