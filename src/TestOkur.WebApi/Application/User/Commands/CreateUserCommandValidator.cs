namespace TestOkur.WebApi.Application.User.Commands
{
	using FluentValidation;
	using TestOkur.Common;

	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
	{
		public CreateUserCommandValidator()
		{
			RuleFor(m => m.Email)
				.EmailAddress()
				.WithMessage(ErrorCodes.InvalidEmailAddress);
			RuleFor(m => m.RegistrarFullName)
				.Name(ErrorCodes.RegistrarNameCannotBeEmpty);
			RuleFor(m => m.UserFirstName)
				.Name(ErrorCodes.FirstNameCannotBeEmpty);
			RuleFor(m => m.UserLastName)
				.Name(ErrorCodes.LastNameCannotBeEmpty);
			RuleFor(m => m.UserPhone)
				.Phone();
			RuleFor(m => m.RegistrarPhone)
				.Phone();
		}
	}
}
