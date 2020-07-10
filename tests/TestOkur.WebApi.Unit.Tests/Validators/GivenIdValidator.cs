namespace TestOkur.WebApi.Unit.Tests.Validators
{
    using FluentAssertions;
    using FluentValidation.Internal;
    using FluentValidation.Validators;
    using TestOkur.WebApi.Validators;
    using Xunit;

    public class GivenIdValidator : ValidatorTestBase
    {
        private readonly IdValidator _validator;
        private readonly PropertyRule _propertyRule;

        public GivenIdValidator()
        {
            _validator = new IdValidator("InvalidId");
            _propertyRule = new PropertyRule(null, null, null, null, null, null);
        }

        [Fact]
        public void WhenValueIsValid_ThenResultShouldBeEmpty()
        {
            var context = new PropertyValidatorContext(DefaultValidationContext, _propertyRule, DefaultPropertyName, 5);
            var result = _validator.Validate(context);
            result.Should().BeEmpty();
        }

        [Fact]
        public void WhenValueIsInvalid_ThenErrorShouldBeReturned()
        {
            var context = new PropertyValidatorContext(DefaultValidationContext, _propertyRule, DefaultPropertyName, 0);
            var result = _validator.Validate(context);
            result.Should().Contain(r => r.ErrorMessage == "InvalidId");
        }
    }
}
