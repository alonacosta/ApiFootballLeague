using System.ComponentModel.DataAnnotations;

namespace ApiFootballLeague.ViewModels
{
    public class RegisterViewModel
    {
        [Required]       
        public string FirstName { get; set; }

        [Required]       
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        //[MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters")]
        //public string Address { get; set; }

        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters")]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

    }
}
