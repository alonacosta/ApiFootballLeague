using System.ComponentModel.DataAnnotations;

namespace ApiFootballLeague.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
