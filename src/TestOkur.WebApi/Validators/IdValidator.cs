namespace TestOkur.WebApi.Validators
{
    using System;
    using FluentValidation.Validators;

    public class IdValidator : PropertyValidator
    {
        public IdValidator(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = Convert.ToInt64(context.PropertyValue);

            return value > 0;
        }
    }
}
