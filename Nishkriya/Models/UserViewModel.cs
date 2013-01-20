using System.ComponentModel.DataAnnotations;

namespace Nishkriya.Models
{
    public class UserViewModel
    {
        [StringLength(100, MinimumLength = 4)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}