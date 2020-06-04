namespace TestOkur.WebApi.Application.Exam.Commands
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ExamModel;
    using TestOkur.Domain.SeedWork;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Exam.Queries;
    using Exam = TestOkur.Domain.Model.ExamModel.Exam;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class EditExamCommandHandler
        : RequestHandlerAsync<EditExamCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IBus _bus;
        private readonly IQueryProcessor _queryProcessor;

        public EditExamCommandHandler(
            IBus bus,
            IApplicationDbContextFactory dbContextFactory,
            IQueryProcessor queryProcessor)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<EditExamCommand> HandleAsync(
            EditExamCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureNotExistsAsync(command, cancellationToken);
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var exam = await GetExamAsync(dbContext, command, cancellationToken);

                if (exam != null)
                {
                    await UpdateExamAsync(dbContext, command, cancellationToken, exam);
                    dbContext.Attach(exam.ExamBookletType);
                    dbContext.Attach(exam.AnswerFormFormat);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await PublishEventAsync(exam, command, cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task UpdateExamAsync(ApplicationDbContext dbContext, EditExamCommand command, CancellationToken cancellationToken, Exam exam)
        {
            exam.Update(
                command.NewName,
                await dbContext.ExamTypes.FirstAsync(e => e.Id == command.NewExamTypeId, cancellationToken),
                command.NewIncorrectEliminationRate,
                command.NewExamDate,
                command.NewApplicableFormTypeCode,
                Enumeration.GetAll<AnswerFormFormat>().First(a => a.Id == command.NewAnswerFormFormat),
                await GetLessonAsync(dbContext, command.NewLessonId),
                Enumeration.GetAll<ExamBookletType>().First(e => e.Id == command.NewExamBookletTypeId),
                command.NewNotes);
        }

        private Task PublishEventAsync(
            Exam exam,
            EditExamCommand command,
            CancellationToken cancellationToken)
        {
            return _bus.Publish(
                new ExamUpdated(exam, command.AnswerKeyOpticalForms),
                cancellationToken);
        }

        private Task<Lesson> GetLessonAsync(ApplicationDbContext dbContext, int id)
        {
            return id <= 0 ?
                Task.FromResult<Lesson>(null) :
                dbContext.Lessons.FirstAsync(l => l.Id == id);
        }

        private Task<Exam> GetExamAsync(
            ApplicationDbContext dbContext,
            EditExamCommand command,
            CancellationToken cancellationToken)
        {
            return dbContext.Exams
                .FirstOrDefaultAsync(
                    l => l.Id == command.ExamId &&
                         EF.Property<int>(l, "CreatedBy") == command.UserId,
                    cancellationToken);
        }

        private async Task EnsureNotExistsAsync(
            EditExamCommand command,
            CancellationToken cancellationToken)
        {
            var list = (await _queryProcessor.ExecuteAsync(
                new GetUserExamsQuery(command.UserId), cancellationToken)).ToList();

            if (list.Any(c => c.Name == command.NewName &&
                     c.Id != command.ExamId))
            {
                throw new ValidationException(ErrorCodes.ExamExists);
            }
        }
    }
}
