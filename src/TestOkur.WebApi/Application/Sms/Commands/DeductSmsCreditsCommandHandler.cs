namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.SmsModel;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class DeductSmsCreditsCommandHandler : RequestHandlerAsync<DeductSmsCreditsCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly ISmsCreditCalculator _smsCreditCalculator;

        public DeductSmsCreditsCommandHandler(ISmsCreditCalculator smsCreditCalculator, IApplicationDbContextFactory dbContextFactory)
        {
            _smsCreditCalculator = smsCreditCalculator;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<DeductSmsCreditsCommand> HandleAsync(
            DeductSmsCreditsCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var user = await dbContext.Users
                .FirstAsync(u => u.Id == command.UserId, cancellationToken);
                user.DeductSmsBalance(_smsCreditCalculator.Calculate(command.SmsBody));
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
