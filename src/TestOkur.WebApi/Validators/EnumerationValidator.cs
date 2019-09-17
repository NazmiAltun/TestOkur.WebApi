namespace TestOkur.WebApi.Validators
{
    using System;
    using System.Linq;
    using FluentValidation.Validators;
    using TestOkur.Domain.SeedWork;

    public class EnumerationValidator<TEnum> : PropertyValidator
        where TEnum : Enumeration, new()
    {
        public EnumerationValidator(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = Convert.ToInt32(context.PropertyValue);
            var all = Enumeration.GetAll<TEnum>();

            return value >= all.Min(x => x.Id) &&
                   value <= all.Max(x => x.Id);
        }
    }
}
