namespace FluentValidation
{
    using TestOkur.Common;
    using TestOkur.Domain.SeedWork;
    using TestOkur.WebApi.Validators;

    public static class FluentValidationExtensions
    {
	    public static IRuleBuilderOptions<T, TProperty> StudentNumber<T, TProperty>(
		    this IRuleBuilder<T, TProperty> ruleBuilder,
		    string errorMessage = ErrorCodes.InvalidStudentNo)
	    {
		    return ruleBuilder.SetValidator(new StudentNumberValidator(errorMessage));
	    }

	    public static IRuleBuilderOptions<T, TProperty> Grade<T, TProperty>(
		    this IRuleBuilder<T, TProperty> ruleBuilder,
		    string errorMessage = ErrorCodes.InvalidGrade)
	    {
		    return ruleBuilder.SetValidator(new GradeValidator(errorMessage));
	    }

	    public static IRuleBuilderOptions<T, TProperty> Name<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string errorMessage = ErrorCodes.NameCannotBeEmpty)
        {
            return ruleBuilder.SetValidator(new NameValidator(errorMessage));
        }

	    public static IRuleBuilderOptions<T, TProperty> Phone<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string errorMessage = ErrorCodes.InvalidPhoneNumber)
        {
            return ruleBuilder.SetValidator(new PhoneValidator(errorMessage));
        }

	    public static IRuleBuilderOptions<T, TProperty> Id<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(new IdValidator(errorMessage));
        }

	    public static IRuleBuilderOptions<T, TProperty> Enumeration<T, TProperty, TEnum>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string errorMessage)
            where TEnum : Enumeration, new()
        {
            return ruleBuilder.SetValidator(new EnumerationValidator<TEnum>(errorMessage));
        }
    }
}
