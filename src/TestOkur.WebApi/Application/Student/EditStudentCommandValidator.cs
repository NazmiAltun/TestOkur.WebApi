namespace TestOkur.WebApi.Application.Student
{
	using FluentValidation;
	using TestOkur.Common;

	public class EditStudentCommandValidator : AbstractValidator<EditStudentCommand>
    {
        public EditStudentCommandValidator()
        {
            RuleFor(m => m.NewFirstName)
                .Name(ErrorCodes.FirstNameCannotBeEmpty);

            RuleFor(m => m.NewLastName)
                .Name(ErrorCodes.LastNameCannotBeEmpty);

            RuleFor(m => m.NewStudentNumber)
                .StudentNumber();

            RuleFor(m => m.NewClassroomId)
                .Id(ErrorCodes.ClassroomDoesNotExist);
        }
    }
}
