namespace TestOkur.WebApi.Unit.Tests.Validators
{
	using FluentAssertions;
	using FluentValidation.Internal;
	using FluentValidation.Validators;
	using TestOkur.WebApi.Validators;
	using Xunit;

	public class GivenGradeValidator
	{
		private readonly GradeValidator _validator;
		private readonly PropertyRule _propertyRule;

		public GivenGradeValidator()
		{
			_validator = new GradeValidator("InvalidGrade");
			_propertyRule = new PropertyRule(null, null, null, null, null, null);
		}

		[Fact]
		public void WhenValueIsValid_ThenResultShouldBeEmpty()
		{
			var context = new PropertyValidatorContext(null, _propertyRule, null, 2);
			var result = _validator.Validate(context);
			result.Should().BeEmpty();
		}

		[Fact]
		public void WhenValueIsInvalid_ThenErrorShouldBeReturned()
		{
			var context = new PropertyValidatorContext(null, _propertyRule, null, 13);
			var result = _validator.Validate(context);
			result.Should().Contain(r => r.ErrorMessage == "InvalidGrade");
		}
	}
}
