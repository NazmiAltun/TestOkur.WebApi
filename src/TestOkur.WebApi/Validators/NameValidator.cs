namespace TestOkur.WebApi.Validators
{
    using FluentValidation.Validators;
    using TestOkur.Domain.Model;

    public class NameValidator : PropertyValidator
    {
        public NameValidator(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            try
            {
                Name.Validate((string)context.PropertyValue);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
