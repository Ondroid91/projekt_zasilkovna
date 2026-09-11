using System.ComponentModel.DataAnnotations;

namespace Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NonNegativeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is decimal number)
            {
                if (number < 0)
                {
                    return new ValidationResult(
                        $"{validationContext.MemberName} must not be less than zero."
                    );
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(
                $"{validationContext.MemberName} must be a decimal number."
            );
        }
    }
}
