namespace TestOkur.WebApi.Unit.Tests.Validators
{
    using FluentAssertions;
    using FluentValidation.Internal;
    using FluentValidation.Validators;
    using TestOkur.Domain.SeedWork;
    using TestOkur.WebApi.Validators;
    using Xunit;

    public class GivenEnumerationValidator
	{
		private readonly EnumerationValidator<Foo> _validator;
		private readonly PropertyRule _propertyRule;

		public GivenEnumerationValidator()
		{
			_validator = new EnumerationValidator<Foo>("InvalidEnum");
			_propertyRule = new PropertyRule(null, null, null, null, null, null);
		}

		[Fact]
		public void WhenValueIsValid_ThenResultShouldBeEmpty()
		{
			var context = new PropertyValidatorContext(null, _propertyRule, null, 1);
			var result = _validator.Validate(context);
			result.Should().BeEmpty();
		}

		[Fact]
		public void WhenValueIsInvalid_ThenErrorShouldBeReturned()
		{
			var context = new PropertyValidatorContext(null, _propertyRule, null, 3);
			var result = _validator.Validate(context);
			result.Should().Contain(r => r.ErrorMessage == "InvalidEnum");
		}

		public class Foo : Enumeration
		{
			public static readonly Foo Bar = new Foo(1, "Bar");

			public static readonly Foo Tar = new Foo(2, "Tar");

			public Foo(int id, string name)
				: base(id, name)
			{
			}

			public Foo()
			{
			}
		}
	}
}
