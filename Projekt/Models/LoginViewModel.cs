using Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Uživatelské jméno je povinné")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Heslo je povinné")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Heslo musí obsahovat minimálně 6 znaků.")]
        [PasswordRequiresNumber]
        [PasswordRequiresUppercase]
        public string Password { get; set; } = "";
    }
}
