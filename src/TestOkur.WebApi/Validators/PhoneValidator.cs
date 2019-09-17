namespace TestOkur.WebApi.Validators
{
    using FluentValidation.Validators;
    using TestOkur.Domain.Model;

    public class PhoneValidator : PropertyValidator
    {
        public PhoneValidator(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            try
            {
                Phone.Validate((string)context.PropertyValue);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
