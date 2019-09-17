namespace TestOkur.WebApi.Application.Classroom
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserClassroomsQuery :
		QueryBase<IReadOnlyCollection<ClassroomReadModel>>,
		ICacheResult
	{
		public string CacheKey => $"Classrooms_{UserId}";

		public TimeSpan CacheDuration => TimeSpan.FromHours(4);
	}
}
