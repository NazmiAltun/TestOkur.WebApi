namespace TestOkur.WebApi.Application.Exam.Commands
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ExamModel;
    using TestOkur.Domain.SeedWork;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Optic.Form;
    using TestOkur.WebApi.Application.Exam.Queries;
    using Exam = TestOkur.Domain.Model.ExamModel.Exam;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class CreateExamCommandHandler : RequestHandlerAsync<CreateExamCommand>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public CreateExamCommandHandler(
            IPublishEndpoint publishEndpoint,
            IApplicationDbContextFactory dbContextFactory,
            IQueryProcessor queryProcessor)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
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

        private Task PublishEventAsync(
            Exam exam,
            IEnumerable<AnswerKeyOpticalForm> answerKeyOpticalForms,
            CancellationToken cancellationToken)
        {
            return _publishEndpoint.Publish(
                new ExamCreated(exam, answerKeyOpticalForms),
                cancellationToken);
        }

        private async Task EnsureNotExistsAsync(
            CreateExamCommand command, CancellationToken cancellationToken)
        {
            var list = (await _queryProcessor.ExecuteAsync(new GetUserExamsQuery(command.UserId), cancellationToken)).ToList();

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
