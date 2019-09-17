namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.StudentModel;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class DeleteContactCommandHandler : RequestHandlerAsync<DeleteContactCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteContactCommandHandler(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [ClearCache(2)]
        public override async Task<DeleteContactCommand> HandleAsync(
            DeleteContactCommand command,
            CancellationToken cancellationToken = default)
        {
            var contact = await GetContactAsync(command, cancellationToken);

            if (contact != null)
            {
                _dbContext.Remove(contact);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<Contact> GetContactAsync(
            DeleteContactCommand command,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Contacts.FirstOrDefaultAsync(
                l => l.Id == command.ContactId && EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
