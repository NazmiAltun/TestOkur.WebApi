namespace TestOkur.WebApi.Validators
{
    using System;
    using FluentValidation.Validators;
    using TestOkur.Domain.Model;

    public class StudentNumberValidator : PropertyValidator
    {
        private readonly string _errorMessage;

        public StudentNumberValidator(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        protected override string GetDefaultMessageTemplate()
        {
            return _errorMessage;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = Convert.ToInt32(context.PropertyValue);

            return StudentNumber.IsValid(value);
        }
    }
}
