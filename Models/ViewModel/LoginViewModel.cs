using System.ComponentModel.DataAnnotations;

namespace Identity1.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(40)]
        [Display(Name ="User Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(40)]
        public string Password { get; set; }
         
        public bool RememberMe { get; set; }

    }
}
