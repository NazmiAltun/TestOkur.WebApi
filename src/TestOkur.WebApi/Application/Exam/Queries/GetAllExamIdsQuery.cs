namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetAllExamIdsQuery : QueryBase<IEnumerable<int>>
    {
        public static GetAllExamIdsQuery Default = new GetAllExamIdsQuery();
    }
}
