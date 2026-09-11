using System.ComponentModel.DataAnnotations;

namespace Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RoleAttribute : ValidationAttribute
    {
        private static readonly string[] AllowedRoles =
        {
            "",
            "Admin",
            "Customer",
            "Messenger",
            "Warehouseman"
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not string role)
                return new ValidationResult("Role must be a string.");

            if (!AllowedRoles.Contains(role))
            {
                return new ValidationResult(
                    $"Role '{role}' is not valid. Allowed roles: {string.Join(", ", AllowedRoles)}"
                );
            }

            return ValidationResult.Success;
        }
    }
}
