namespace TestOkur.WebApi.Application.User.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task ExtendUserSubscriptionAsync(string id, CancellationToken cancellationToken);

        Task DeleteUserAsync(string id, CancellationToken cancellationToken);

		Task ActivateUserAsync(string email, CancellationToken cancellationToken);

		Task RegisterUserAsync(CreateCustomerUserModel model, CancellationToken cancellationToken = default);

		Task UpdateUserAsync(UpdateUserModel model, CancellationToken cancellationToken = default);

		Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default);
	}
}
