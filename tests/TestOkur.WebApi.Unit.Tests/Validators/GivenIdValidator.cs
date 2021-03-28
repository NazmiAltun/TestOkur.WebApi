namespace TestOkur.WebApi.Unit.Tests.Validators
{
    using FluentAssertions;
    using FluentValidation.Internal;
    using FluentValidation.Validators;
    using TestOkur.WebApi.Validators;
    using Xunit;

    public class GivenIdValidator
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
            var context = new PropertyValidatorContext(null, _propertyRule, null, 5);
            var result = _validator.Validate(context);
            result.Should().BeEmpty();
        }

        [Fact(Skip = "Fix this later")]
        public void WhenValueIsInvalid_ThenErrorShouldBeReturned()
        {
            var context = new PropertyValidatorContext(null, _propertyRule, null, 0);
            var result = _validator.Validate(context);
            result.Should().Contain(r => r.ErrorMessage == "InvalidId");
        }
    }
}
