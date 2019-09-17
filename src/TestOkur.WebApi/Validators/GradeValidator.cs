namespace TestOkur.WebApi.Validators
{
    using System;
    using FluentValidation.Validators;
    using TestOkur.Domain.Model;

    public class GradeValidator : PropertyValidator
	{
		public GradeValidator(string errorMessage)
			: base(errorMessage)
		{
		}

		protected override bool IsValid(PropertyValidatorContext context)
		{
			var value = Convert.ToInt32(context.PropertyValue);

			return Grade.IsValid(value);
		}
	}
}
