namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ExamModel;
    using TestOkur.Domain.SeedWork;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Exam.Queries;
    using Exam = TestOkur.Domain.Model.ExamModel.Exam;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class EditExamCommandHandler
		: RequestHandlerAsync<EditExamCommand>
	{
		private readonly IQueryProcessor _queryProcessor;
		private readonly ApplicationDbContext _dbContext;
		private readonly IPublishEndpoint _publishEndpoint;

		public EditExamCommandHandler(
			ApplicationDbContext dbContext,
			IPublishEndpoint publishEndpoint,
			IQueryProcessor queryProcessor)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_queryProcessor = queryProcessor;
		}

		[Idempotent(1)]
		[ClearCache(3)]
		public override async Task<EditExamCommand> HandleAsync(
			EditExamCommand command,
			CancellationToken cancellationToken = default)
		{
			await EnsureNotExistsAsync(command, cancellationToken);
			var exam = await GetExamAsync(command, cancellationToken);

			if (exam != null)
			{
				await UpdateExamAsync(command, cancellationToken, exam);
				_dbContext.Attach(exam.ExamBookletType);
				_dbContext.Attach(exam.AnswerFormFormat);
				await _dbContext.SaveChangesAsync(cancellationToken);
				await PublishEventAsync(exam, command, cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task UpdateExamAsync(EditExamCommand command, CancellationToken cancellationToken, Exam exam)
		{
			exam.Update(
				command.NewName,
				await _dbContext.ExamTypes.FirstAsync(e => e.Id == command.NewExamTypeId, cancellationToken),
				command.NewIncorrectEliminationRate,
				command.NewExamDate,
				command.NewApplicableFormTypeCode,
				Enumeration.GetAll<AnswerFormFormat>().First(a => a.Id == command.NewAnswerFormFormat),
				await GetLessonAsync(command.NewLessonId),
				Enumeration.GetAll<ExamBookletType>().First(e => e.Id == command.NewExamBookletTypeId),
				command.NewNotes);
		}

		private async Task PublishEventAsync(
			Exam exam,
			EditExamCommand command,
			CancellationToken cancellationToken)
		{
			await _publishEndpoint.Publish(
				new ExamUpdated(exam, command.AnswerKeyOpticalForms),
				cancellationToken);
		}

		private async Task<Lesson> GetLessonAsync(int id)
		{
			return id <= 0 ?
				null :
				await _dbContext.Lessons.FirstAsync(l => l.Id == id);
		}

		private async Task<Exam> GetExamAsync(
			EditExamCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Exams
				.FirstOrDefaultAsync(
					l => l.Id == command.ExamId,
					cancellationToken);
		}

		private async Task EnsureNotExistsAsync(
			EditExamCommand command,
			CancellationToken cancellationToken)
		{
			var list = (await _queryProcessor.ExecuteAsync(
				new GetUserExamsQuery(), cancellationToken)).ToList();

			if (list.Any(c => c.Name == command.NewName &&
					 c.Id != command.ExamId))
			{
				throw new ValidationException(ErrorCodes.ExamExists);
			}
		}
	}
}
