namespace TestOkur.WebApi.Application.Exam.Commands
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using MassTransit;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class DeleteExamCommandHandler : RequestHandlerAsync<DeleteExamCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IPublishEndpoint _publishEndpoint;

		public DeleteExamCommandHandler(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
		}

		[Populate(1)]
		[ClearCache(2)]
		public override async Task<DeleteExamCommand> HandleAsync(
			DeleteExamCommand command,
			CancellationToken cancellationToken = default)
		{
			var exam = await GetAsync(command, cancellationToken);

			if (exam != null)
			{
				_dbContext.Remove(exam);
				await _dbContext.SaveChangesAsync(cancellationToken);
				await PublishEventAsync(command.ExamId, cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(int id, CancellationToken cancellationToken)
		{
			await _publishEndpoint.Publish(
				new ExamDeleted(id),
				cancellationToken);
		}

		private async Task<Domain.Model.ExamModel.Exam> GetAsync(
			DeleteExamCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Exams.FirstOrDefaultAsync(
				l => l.Id == command.ExamId &&
				     EF.Property<int>(l, "CreatedBy") == command.UserId,
				cancellationToken);
		}
	}
}
