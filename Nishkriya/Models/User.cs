using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nishkriya.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 4)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 10)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        public bool IsAdmin { get; set; }
    }
}