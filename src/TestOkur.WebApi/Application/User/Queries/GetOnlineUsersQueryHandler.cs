using Paramore.Darker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestOkur.WebApi.Application.User.Queries
{
    public sealed class GetOnlineUsersQueryHandler : QueryHandlerAsync<GetOnlineUsersQuery, IReadOnlyCollection<string>>
    {
        private readonly Dictionary<string, DateTime> _userCache;

        public GetOnlineUsersQueryHandler()
        {
        }

        public override Task<IReadOnlyCollection<string>> ExecuteAsync(GetOnlineUsersQuery query, CancellationToken cancellationToken = default)
        {
        }
    }
}
