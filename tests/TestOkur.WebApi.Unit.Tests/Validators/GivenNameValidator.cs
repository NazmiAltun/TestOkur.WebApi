namespace TestOkur.WebApi.Unit.Tests.Validators
{
    using FluentAssertions;
    using FluentValidation.Internal;
    using FluentValidation.Validators;
    using TestOkur.Common;
    using TestOkur.WebApi.Validators;
    using Xunit;

    public class GivenNameValidator : ValidatorTestBase
    {
        private readonly NameValidator _validator;
        private readonly PropertyRule _propertyRule;

        public GivenNameValidator()
        {
            _validator = new NameValidator(ErrorCodes.NameCannotBeEmpty);
            _propertyRule = new PropertyRule(null, null, null, null, null, null);
        }

        [Fact]
        public void WhenValueIsValid_ThenResultShouldBeEmpty()
        {
            var context = new PropertyValidatorContext(DefaultValidationContext, _propertyRule, DefaultPropertyName, "John");
            var result = _validator.Validate(context);
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("VeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongNameVeryLongName")]
        public void WhenValueIsInvalid_ThenErrorShouldBeReturned(string name)
        {
            var context = new PropertyValidatorContext(DefaultValidationContext, _propertyRule, DefaultPropertyName, name);
            var result = _validator.Validate(context);
            result.Should().Contain(r => r.ErrorMessage == ErrorCodes.NameCannotBeEmpty);
        }
    }
}
