namespace TestOkur.WebApi.Application.User.Commands
{
	using FluentValidation;
	using TestOkur.Common;

	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
		public UpdateUserCommandValidator()
		{
			RuleFor(m => m.SchoolName)
				.Name(ErrorCodes.SchoolNameCannotBeEmpty);
			RuleFor(m => m.MobilePhone)
				.NotEmpty();
			RuleFor(m => m.CityId)
				.Id(ErrorCodes.CityIdCannotBeEmpty);
			RuleFor(m => m.DistrictId)
				.Id(ErrorCodes.DistrictIdCannotBeEmpty);
		}
	}
}
