namespace TestOkur.WebApi.Application.Contact
{
    using Paramore.Brighter;
    using Paramore.Darker;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class CreateContactCommandHandler
        : RequestHandlerAsync<CreateContactCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IQueryProcessor _queryProcessor;

        public CreateContactCommandHandler(IApplicationDbContextFactory dbContextFactory, IQueryProcessor queryProcessor)
        {
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(2)]
        [ClearCache(4)]
        public override async Task<CreateContactCommand> HandleAsync(
            CreateContactCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureContactDoesNotExist(command, cancellationToken);
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                dbContext.Contacts.Add(command.ToDomainModel());
                dbContext.Attach(command.ToDomainModel().ContactType);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureContactDoesNotExist(
            CreateContactCommand command,
            CancellationToken cancellationToken)
        {
            var query = new GetUserContactsQuery(command.UserId);
            var list = await _queryProcessor.ExecuteAsync(query, cancellationToken);

            if (list.Any(c => c.Phone == command.Phone))
            {
                throw new ValidationException(ErrorCodes.ContactExists);
            }
        }
    }
}
