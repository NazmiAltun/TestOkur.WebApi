namespace TestOkur.WebApi.Application.Sms.Commands
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;

    public class AddSmsCreditsCommandHandler : RequestHandlerAsync<AddSmsCreditsCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public AddSmsCreditsCommandHandler(IPublishEndpoint publishEndpoint, IApplicationDbContextFactory dbContextFactory)
        {
            _publishEndpoint = publishEndpoint;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<AddSmsCreditsCommand> HandleAsync(
            AddSmsCreditsCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var user = await dbContext.Users
                    .FirstAsync(
                        u => u.Id == command.UserId,
                        cancellationToken);
                user.AddSmsBalance(command.Amount);
                await dbContext.SaveChangesAsync(cancellationToken);
                await _publishEndpoint.Publish(
                    new SmsCreditAdded(
                        command.Amount,
                        user.SmsBalance,
                        user.FirstName.Value,
                        user.LastName.Value,
                        user.Email,
                        user.Phone,
                        command.Gift,
                        (int)user.Id,
                        user.SubjectId), cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
