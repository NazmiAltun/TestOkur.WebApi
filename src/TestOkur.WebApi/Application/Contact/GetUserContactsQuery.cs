namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserContactsQuery :
        QueryBase<IReadOnlyCollection<ContactReadModel>>,
        ICacheResult
    {
        public string CacheKey => $"Contacts_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
