namespace TestOkur.WebApi.Application.Exam.Commands
{
    using FluentValidation;

    public class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
    {
        public CreateExamCommandValidator()
        {
            RuleFor(m => m.Name).Name();
        }
    }
}
