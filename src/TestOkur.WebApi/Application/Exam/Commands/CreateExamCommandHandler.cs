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
    using Paramore.Darker;
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
		private readonly IQueryProcessor _queryProcessor;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly ApplicationDbContext _dbContext;

		public CreateExamCommandHandler(
			ApplicationDbContext dbContext,
			IQueryProcessor queryProcessor,
			IPublishEndpoint publishEndpoint)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_queryProcessor = queryProcessor;
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
		}

		[Idempotent(2)]
		[ClearCache(4)]
		public override async Task<CreateExamCommand> HandleAsync(
			CreateExamCommand command,
			CancellationToken cancellationToken = default)
		{
			await EnsureNotExistsAsync(command.Name, cancellationToken);

			var exam = new Exam(
				command.Name,
				command.ExamDate,
				await _dbContext.ExamTypes.FirstAsync(e => e.Id == command.ExamTypeId, cancellationToken),
				Enumeration.GetAll<ExamBookletType>().First(e => e.Id == command.ExamBookletTypeId),
				command.IncorrectEliminationRate,
				Enumeration.GetAll<AnswerFormFormat>().First(a => a.Id == command.AnswerFormFormat),
				await GetLessonAsync(command.LessonId),
				command.ApplicableFormTypeCode,
				command.Notes);

			_dbContext.Attach(exam.ExamBookletType);
			_dbContext.Attach(exam.AnswerFormFormat);
			_dbContext.Exams.Add(exam);
			await _dbContext.SaveChangesAsync(cancellationToken);
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
			string name, CancellationToken cancellationToken)
		{
			var list = (await _queryProcessor.ExecuteAsync(
				new GetUserExamsQuery(), cancellationToken)).ToList();

			if (list.Any(c => c.Name == name))
			{
				throw new ValidationException(ErrorCodes.ExamExists);
			}
		}

		private async Task<Lesson> GetLessonAsync(int id)
		{
			return id <= 0 ?
				null :
				await _dbContext.Lessons.FirstAsync(l => l.Id == id);
		}
	}
}
