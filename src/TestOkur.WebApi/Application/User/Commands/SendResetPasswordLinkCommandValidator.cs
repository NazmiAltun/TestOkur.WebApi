namespace TestOkur.WebApi.Application.User.Commands
{
	using FluentValidation;
	using TestOkur.Common;

	public class SendResetPasswordLinkCommandValidator : AbstractValidator<SendResetPasswordLinkCommand>
	{
		public SendResetPasswordLinkCommandValidator()
		{
			RuleFor(m => m.Email)
				.EmailAddress()
				.WithMessage(ErrorCodes.InvalidEmailAddress);
		}
	}
}
