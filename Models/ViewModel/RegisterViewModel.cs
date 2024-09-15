using System.ComponentModel.DataAnnotations;

namespace Identity1.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(40)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [MaxLength(40)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [MaxLength(40)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


        public string Phone { get; set; }
        public string City { get; set; }
 
    }
}
