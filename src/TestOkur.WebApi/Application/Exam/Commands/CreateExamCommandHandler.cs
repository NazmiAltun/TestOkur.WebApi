namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ExamModel;
    using TestOkur.Domain.SeedWork;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.Optic.Form;
    using TestOkur.WebApi.Application.Exam.Queries;
    using Exam = TestOkur.Domain.Model.ExamModel.Exam;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class CreateExamCommandHandler : RequestHandlerAsync<CreateExamCommand>
    {
        private readonly IProcessor _processor;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public CreateExamCommandHandler(
            IProcessor processor,
            IPublishEndpoint publishEndpoint,
            IApplicationDbContextFactory dbContextFactory)
        {
            _processor = processor;
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(2)]
        [ClearCache(4)]
        public override async Task<CreateExamCommand> HandleAsync(
            CreateExamCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureNotExistsAsync(command, cancellationToken);
            Exam exam = null;

            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                exam = new Exam(
                    command.Name,
                    command.ExamDate,
                    await dbContext.ExamTypes.FirstAsync(e => e.Id == command.ExamTypeId, cancellationToken),
                    Enumeration.GetAll<ExamBookletType>().First(e => e.Id == command.ExamBookletTypeId),
                    command.IncorrectEliminationRate,
                    Enumeration.GetAll<AnswerFormFormat>().First(a => a.Id == command.AnswerFormFormat),
                    await GetLessonAsync(dbContext, command.LessonId),
                    command.ApplicableFormTypeCode,
                    command.Notes,
                    command.Shared);

                dbContext.Attach(exam.ExamBookletType);
                dbContext.Attach(exam.AnswerFormFormat);
                dbContext.Exams.Add(exam);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await PublishEventAsync(exam, command.AnswerKeyOpticalForms, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(
            Exam exam,
            IEnumerable<AnswerKeyOpticalForm> answerKeyOpticalForms,
            CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new ExamCreated(exam, answerKeyOpticalForms),
                cancellationToken);
        }

        private async Task EnsureNotExistsAsync(
            CreateExamCommand command, CancellationToken cancellationToken)
        {
            var list = (await _processor.ExecuteAsync<GetUserExamsQuery, IReadOnlyCollection<ExamReadModel>>(
                new GetUserExamsQuery(command.UserId), cancellationToken)).ToList();

            if (list.Any(c => c.Name == command.Name))
            {
                throw new ValidationException(ErrorCodes.ExamExists);
            }
        }

        private async Task<Lesson> GetLessonAsync(ApplicationDbContext dbContext, int id)
        {
            return id <= 0 ?
                null :
                await dbContext.Lessons.FirstAsync(l => l.Id == id);
        }
    }
}
