using System;
using System.ComponentModel.DataAnnotations;

namespace No_Core_Auth.Model
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
