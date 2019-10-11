namespace TestOkur.WebApi.Application.Classroom
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetUserClassroomsQuery :
        QueryBase<IReadOnlyCollection<ClassroomReadModel>>,
        ICacheResult
    {
        public GetUserClassroomsQuery()
        {
        }

        public GetUserClassroomsQuery(int userId)
            : base(userId)
        {
        }

        public string CacheKey => $"Classrooms_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
