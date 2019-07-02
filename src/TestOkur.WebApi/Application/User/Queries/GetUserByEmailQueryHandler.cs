namespace TestOkur.WebApi.Application.User.Queries
{
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Paramore.Darker;
	using Paramore.Darker.QueryLogging;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class GetUserByEmailQueryHandler : QueryHandlerAsync<GetUserByEmailQuery, UserReadModel>
	{
		private readonly IQueryProcessor _queryProcessor;

		public GetUserByEmailQueryHandler(IQueryProcessor queryProcessor)
		{
			_queryProcessor = queryProcessor;
		}

		[PopulateQuery(1)]
		[QueryLogging(2)]
		public override async Task<UserReadModel> ExecuteAsync(
			GetUserByEmailQuery query,
			CancellationToken cancellationToken = default)
		{
			var users = await _queryProcessor.ExecuteAsync(
				new GetAllUsersQuery(),
				cancellationToken);

			return string.IsNullOrEmpty(query.Email)
				? users.First(u => u.Id == query.UserId)
				: users.First(u => u.Email == query.Email);
		}
	}
}
