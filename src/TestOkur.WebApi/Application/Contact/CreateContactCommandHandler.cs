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
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IProcessor _processor;

        public CreateContactCommandHandler(
            IApplicationDbContextFactory dbContextFactory,
            IProcessor processor)
        {
            _dbContextFactory = dbContextFactory;
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [Idempotent(2)]
        [ClearCache(4)]
        public override async Task<CreateContactCommand> HandleAsync(
            CreateContactCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureContactDoesNotExist(command, cancellationToken);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
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
            var query = new GetUserContactsQuery();
            var list = await _processor.ExecuteAsync<GetUserContactsQuery, IReadOnlyCollection<ContactReadModel>>(query, cancellationToken);

            if (list.Any(c => c.Phone == command.Phone))
            {
                throw new ValidationException(ErrorCodes.ContactExists);
            }
        }
    }
}
