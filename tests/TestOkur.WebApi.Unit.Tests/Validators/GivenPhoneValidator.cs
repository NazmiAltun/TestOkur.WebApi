namespace TestOkur.WebApi.Unit.Tests.Validators
{
    using FluentAssertions;
    using FluentValidation.Internal;
    using FluentValidation.Validators;
    using TestOkur.Common;
    using TestOkur.WebApi.Validators;
    using Xunit;

    public class GivenPhoneValidator : ValidatorTestBase
    {
        private readonly PhoneValidator _validator;
        private readonly PropertyRule _propertyRule;

        public GivenPhoneValidator()
        {
            _validator = new PhoneValidator(ErrorCodes.InvalidPhoneNumber);
            _propertyRule = new PropertyRule(null, null, null, null, null, null);
        }

        [Fact]
        public void WhenValueIsValid_ThenResultShouldBeEmpty()
        {
            var context = new PropertyValidatorContext(DefaultValidationContext, _propertyRule, DefaultPropertyName, "5544205163");
            var result = _validator.Validate(context);
            result.Should().BeEmpty();
        }

        [Fact]
        public void WhenValueIsInvalid_ThenErrorShouldBeReturned()
        {
            var context = new PropertyValidatorContext(DefaultValidationContext, _propertyRule, DefaultPropertyName, "05324589966");
            var result = _validator.Validate(context);
            result.Should().Contain(r => r.ErrorMessage == ErrorCodes.InvalidPhoneNumber);
        }
    }
}
