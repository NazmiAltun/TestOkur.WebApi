namespace TestOkur.WebApi.Application.User.Commands
{
    using FluentValidation;
    using TestOkur.Common;

    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
	{
		public ActivateUserCommandValidator()
		{
			RuleFor(m => m.UserId)
				.Id(ErrorCodes.InvalidUserId);
		}
	}
}
