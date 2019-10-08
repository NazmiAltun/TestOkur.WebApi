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

            var writeModels = new List<WriteModel<SchoolResult>>();

            foreach (var result in results)
            {
                var model = new ReplaceOneModel<SchoolResult>(
                    Builders<SchoolResult>.Filter.Eq(x => x.ExamId, result.ExamId) &
                    Builders<SchoolResult>.Filter.Eq(x => x.SchoolId, result.SchoolId),
                    result)
                {
                    IsUpsert = true,
                };

                writeModels.Add(model);
            }

            await _context.SchoolResults.BulkWriteAsync(writeModels);
        }

        public async Task<IEnumerable<SchoolResult>> GetByExamId(int examId)
        {
            return await _context.SchoolResults
                .Find(Builders<SchoolResult>.Filter.Eq(x => x.ExamId, examId))
                .ToListAsync();
        }
    }
}
