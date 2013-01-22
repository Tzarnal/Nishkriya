using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nishkriya.Models.ViewModels
{
    public class NewUserViewModel
    {
        [StringLength(20, MinimumLength = 4)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 10)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Secret", Description = "Do you know the secret?")]
        public string Secret { get; set; }
    }
}