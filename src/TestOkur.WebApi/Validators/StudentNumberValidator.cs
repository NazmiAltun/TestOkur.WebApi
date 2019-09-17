namespace TestOkur.WebApi.Validators
{
    using System;
    using FluentValidation.Validators;
    using TestOkur.Domain.Model;

    public class StudentNumberValidator : PropertyValidator
    {
        public StudentNumberValidator(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = Convert.ToInt32(context.PropertyValue);

            return StudentNumber.IsValid(value);
        }
    }
}
