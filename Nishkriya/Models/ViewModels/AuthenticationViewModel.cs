using System.ComponentModel.DataAnnotations;

namespace Nishkriya.Models
{
    public class AuthenticationViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 10)]
        public string Password { get; set; }
    }
}