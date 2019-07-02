namespace TestOkur.WebApi.Application.Student
{
	using FluentValidation;
	using TestOkur.Common;

	public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(m => m.FirstName)
                .Name(ErrorCodes.FirstNameCannotBeEmpty);

            RuleFor(m => m.LastName)
                .Name(ErrorCodes.LastNameCannotBeEmpty);

            RuleFor(m => m.StudentNumber)
                .StudentNumber();

            RuleFor(m => m.ClassroomId)
                .Id(ErrorCodes.ClassroomDoesNotExist);
        }
    }
}
