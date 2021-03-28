namespace TestOkur.WebApi.Unit.Tests.Validators
{
    using FluentAssertions;
    using FluentValidation.Internal;
    using FluentValidation.Validators;
    using TestOkur.Common;
    using TestOkur.WebApi.Validators;
    using Xunit;

    public class GivenPhoneValidator
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
            var context = new PropertyValidatorContext(null, _propertyRule, null, "5544205163");
            var result = _validator.Validate(context);
            result.Should().BeEmpty();
        }

        [Fact(Skip = "Fix this later")]
        public void WhenValueIsInvalid_ThenErrorShouldBeReturned()
        {
            var context = new PropertyValidatorContext(null, _propertyRule, null, "05324589966");
            var result = _validator.Validate(context);
            result.Should().Contain(r => r.ErrorMessage == ErrorCodes.InvalidPhoneNumber);
        }
    }
}
