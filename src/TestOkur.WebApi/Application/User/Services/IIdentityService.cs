namespace TestOkur.WebApi.Application.User.Services
{
	using System.Threading;
	using System.Threading.Tasks;
	using TestOkur.Domain.Model;

	public interface IIdentityService
	{
		Task DeleteUserAsync(string id, CancellationToken cancellationToken);

		Task ActivateUserAsync(Email email, CancellationToken cancellationToken);

		Task RegisterUserAsync(CreateCustomerUserModel model, CancellationToken cancellationToken = default);

		Task UpdateUserAsync(UpdateUserModel model, CancellationToken cancellationToken = default);

		Task<string> GeneratePasswordResetTokenAsync(Email email, CancellationToken cancellationToken = default);
	}
}
