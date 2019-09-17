namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;

    public class AddSmsCreditsCommandHandler : RequestHandlerAsync<AddSmsCreditsCommand>
	{
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly IPublishEndpoint _publishEndpoint;

		public AddSmsCreditsCommandHandler(ApplicationDbContext applicationDbContext, IPublishEndpoint publishEndpoint)
		{
			_applicationDbContext = applicationDbContext;
			_publishEndpoint = publishEndpoint;
		}

		[Idempotent(1)]
		[ClearCache(2)]
		public override async Task<AddSmsCreditsCommand> HandleAsync(
			AddSmsCreditsCommand command,
			CancellationToken cancellationToken = default)
		{
			var user = await _applicationDbContext.Users
				.FirstAsync(
					u => u.Id == command.UserId,
					cancellationToken);
			user.AddSmsBalance(command.Amount);
			await _applicationDbContext.SaveChangesAsync(cancellationToken);
			await _publishEndpoint.Publish(
				new SmsCreditAdded(
					command.Amount,
					user.SmsBalance,
					user.FirstName.Value,
					user.LastName.Value,
					user.Email,
					user.Phone), cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}
	}
}
