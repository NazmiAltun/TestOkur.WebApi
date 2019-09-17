namespace TestOkur.WebApi.Application.Contact
{
    using Paramore.Brighter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class CreateContactCommandHandler
        : RequestHandlerAsync<CreateContactCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProcessor _processor;

        public CreateContactCommandHandler(
            ApplicationDbContext dbContext,
            IProcessor processor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [Idempotent(2)]
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
            var list = await _processor.ExecuteAsync<GetUserContactsQuery, IReadOnlyCollection<ContactReadModel>>(query, cancellationToken);

            if (list.Any(c => c.Phone == command.Phone))
            {
                throw new ValidationException(ErrorCodes.ContactExists);
            }
        }
    }
}
