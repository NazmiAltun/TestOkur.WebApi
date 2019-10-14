namespace TestOkur.Report.Infrastructure.Repositories
{
    using System;
    using CacheManager.Core;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Report.Configuration;
    using TestOkur.Report.Domain;

    public class SchoolResultRepository : ISchoolResultRepository
    {
        private const string BaseCacheKey = "SchoolResults";

        private readonly TestOkurContext _context;
        private readonly ICacheManager<IEnumerable<SchoolResult>> _cache;

        public SchoolResultRepository(ReportConfiguration configuration, ICacheManager<IEnumerable<SchoolResult>> cache)
        {
            _cache = cache;
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
            _cache.Remove($"{BaseCacheKey}-{results.First().ExamId}");
        }

        public async Task<IEnumerable<SchoolResult>> GetByExamId(int examId)
        {
            var key = $"{BaseCacheKey}-{examId}";
            var results = _cache.Get(key);

            if (results != null)
            {
                return results;
            }

            results = await _context.SchoolResults
                .Find(Builders<SchoolResult>.Filter.Eq(x => x.ExamId, examId))
                .ToListAsync();
            _cache.Add(new CacheItem<IEnumerable<SchoolResult>>(
                key, results, ExpirationMode.Absolute, TimeSpan.MaxValue));
            return results;
        }
    }
}
