namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetAllExamIdsQuery : QueryBase<IEnumerable<int>>
    {
    }
}
