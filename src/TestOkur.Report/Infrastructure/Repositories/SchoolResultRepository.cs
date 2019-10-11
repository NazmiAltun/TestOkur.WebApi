namespace TestOkur.Report.Infrastructure.Repositories
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Report.Configuration;
    using TestOkur.Report.Domain;

    public class SchoolResultRepository : ISchoolResultRepository
    {
        private readonly TestOkurContext _context;

        public SchoolResultRepository(ReportConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public async Task AddOrUpdateManyAsync(IEnumerable<SchoolResult> results)
        {
            if (results == null || !results.Any())
            {
                return;
            }
            
            var filter = Builders<SchoolResult>.Filter.Eq(x => x.ExamId, results.First().ExamId);
            await _context.SchoolResults.DeleteManyAsync(filter);
            await _context.SchoolResults.InsertManyAsync(results);
        }

        public async Task<IEnumerable<SchoolResult>> GetByExamId(int examId)
        {
            return await _context.SchoolResults
                .Find(Builders<SchoolResult>.Filter.Eq(x => x.ExamId, examId))
                .ToListAsync();
        }
    }
}
