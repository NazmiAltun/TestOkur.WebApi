namespace TestOkur.WebApi.Application.Contact
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Paramore.Brighter;
	using Paramore.Darker;
	using TestOkur.Common;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class CreateContactCommandHandler
		: RequestHandlerAsync<CreateContactCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IQueryProcessor _queryProcessor;

		public CreateContactCommandHandler(
			ApplicationDbContext dbContext,
			IQueryProcessor queryProcessor)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[Idempotent(2)]
		[Populate(3)]
		[ClearCache(4)]
		public override async Task<CreateContactCommand> HandleAsync(
			CreateContactCommand command,
			CancellationToken cancellationToken = default)
		{
			await EnsureContactDoesNotExist(command, cancellationToken);
			_dbContext.Contacts.Add(command.ToDomainModel());
			_dbContext.Attach(command.ToDomainModel().ContactType);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task EnsureContactDoesNotExist(
			CreateContactCommand command,
			CancellationToken cancellationToken)
		{
			var query = new GetUserContactsQuery();
			var list = await _queryProcessor.ExecuteAsync(query, cancellationToken);

			if (list.Any(c => c.Phone == command.Phone))
			{
				throw new ValidationException(ErrorCodes.ContactExists);
			}
		}
	}
}
