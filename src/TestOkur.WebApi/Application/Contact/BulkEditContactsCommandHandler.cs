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
    using TestOkur.Infrastructure.Cqrs;

    public class BulkEditContactsCommandHandler : RequestHandlerAsync<BulkEditContactsCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public BulkEditContactsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<BulkEditContactsCommand> HandleAsync(
            BulkEditContactsCommand command,
            CancellationToken cancellationToken = default)
        {
            var contactTypes = Domain.SeedWork.Enumeration.GetAll<ContactType>();
            foreach (var subCommand in command.Commands)
            {
                var contact = await _dbContext.Contacts
                    .FirstAsync(c => c.Id == subCommand.ContactId, cancellationToken);
                contact.Update(
                    subCommand.FirstName,
                    subCommand.LastName,
                    subCommand.Phone,
                    contactTypes.First(t => t.Id == subCommand.ContactType),
                    subCommand.Labels);
            }

            _dbContext.AttachRange(contactTypes);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
