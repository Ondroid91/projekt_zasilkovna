using System.ComponentModel.DataAnnotations;
using Domain.Validations;

namespace Projekt.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Uživatelské jméno je povinné")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Heslo je povinné")]
        [MinLength(6, ErrorMessage = "Heslo musí obsahovat minimálně 6 znaků.")]
        [PasswordRequiresNumber]
        [PasswordRequiresUppercase]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Email není platný")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Telefon je povinný")]
        [Phone(ErrorMessage = "Telefonní číslo není platné")]
        public string Phone { get; set; } = "";

        [Role]
        public string Role { get; set; } = "Customer";

        public int? WorkplaceId { get; set; }
    }
}
