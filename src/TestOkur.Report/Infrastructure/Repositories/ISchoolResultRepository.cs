namespace TestOkur.Report.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Report.Domain;

    public interface ISchoolResultRepository
    {
        Task AddOrUpdateManyAsync(IEnumerable<SchoolResult> results);

        Task<IEnumerable<SchoolResult>> GetByExamId(int examId);
    }
}