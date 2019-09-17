namespace TestOkur.WebApi.Application.Classroom
{
    using FluentValidation;

    public class CreateClassroomCommandValidator : AbstractValidator<CreateClassroomCommand>
	{
		public CreateClassroomCommandValidator()
		{
			RuleFor(m => m.Name).Name();
			RuleFor(m => m.Grade).Grade();
		}
	}
}
