using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class PasswordRequiresNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not string password)
                return new ValidationResult("Password must be a string.");

            if (!password.Any(char.IsDigit))
                return new ValidationResult("Heslo musí obsahovat minimálně jedno číslo.");

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class PasswordRequiresUppercaseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not string password)
                return new ValidationResult("Password must be a string.");

            if (!password.Any(char.IsUpper))
                return new ValidationResult("Heslo musí obsahovat minimálně jedno velké písmeno.");

            return ValidationResult.Success;
        }
    }
}
