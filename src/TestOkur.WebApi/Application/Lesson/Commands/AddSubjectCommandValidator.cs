namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using FluentValidation;
    using TestOkur.Common;

    public class AddSubjectCommandValidator : AbstractValidator<AddSubjectCommand>
    {
        public AddSubjectCommandValidator()
        {
            RuleFor(m => m.Name).Name();
            RuleFor(m => m.UnitId).Id(ErrorCodes.UnitDoesNotExist);
        }
    }
}
