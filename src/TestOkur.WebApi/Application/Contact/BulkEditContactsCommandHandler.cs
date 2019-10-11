namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.StudentModel;
    using TestOkur.Infrastructure.CommandsQueries;

    public class BulkEditContactsCommandHandler : RequestHandlerAsync<BulkEditContactsCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public BulkEditContactsCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<BulkEditContactsCommand> HandleAsync(
            BulkEditContactsCommand command,
            CancellationToken cancellationToken = default)
        {
            var contactTypes = Domain.SeedWork.Enumeration.GetAll<ContactType>();
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                foreach (var subCommand in command.Commands)
                {
                    var contact = await dbContext.Contacts
                        .FirstAsync(c => c.Id == subCommand.ContactId, cancellationToken);
                    contact.Update(
                        subCommand.FirstName,
                        subCommand.LastName,
                        subCommand.Phone,
                        contactTypes.First(t => t.Id == subCommand.ContactType),
                        subCommand.Labels);
                }

                dbContext.AttachRange(contactTypes);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
