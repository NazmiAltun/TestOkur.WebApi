namespace TestOkur.WebApi.Application.Sms.Commands
{
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Domain.Model.SmsModel;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class DeductSmsCreditsCommandHandler : RequestHandlerAsync<DeductSmsCreditsCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ISmsCreditCalculator _smsCreditCalculator;

        public DeductSmsCreditsCommandHandler(
            ApplicationDbContext applicationDbContext,
            ISmsCreditCalculator smsCreditCalculator)
        {
            _applicationDbContext = applicationDbContext;
            _smsCreditCalculator = smsCreditCalculator;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<DeductSmsCreditsCommand> HandleAsync(
            DeductSmsCreditsCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _applicationDbContext.Users
                .FirstAsync(u => u.Id == command.UserId, cancellationToken);
            user.DeductSmsBalance(_smsCreditCalculator.Calculate(command.SmsBody));
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
