using FluentValidation;

namespace TestOkur.WebApi.Unit.Tests.Validators
{
    public abstract class ValidatorTestBase
    {
        protected const string DefaultPropertyName = "Foo";

        protected readonly ValidationContext<string> DefaultValidationContext = new ValidationContext<string>(DefaultPropertyName);
    }
}
