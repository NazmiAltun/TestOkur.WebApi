namespace TestOkur.Report.Infrastructure.Repositories
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Configuration;

    public class AnswerKeyOpticalFormRepository : IAnswerKeyOpticalFormRepository
    {
        private readonly TestOkurContext _context;

        public AnswerKeyOpticalFormRepository(ReportConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public Task AddOrUpdateManyAsync(IEnumerable<AnswerKeyOpticalForm> forms)
        {
            var writeModels = new List<WriteModel<AnswerKeyOpticalForm>>(forms.Count());

            foreach (var form in forms)
            {
                var model = new ReplaceOneModel<AnswerKeyOpticalForm>(
                    Builders<AnswerKeyOpticalForm>.Filter.Eq(x => x.Id, form.Id),
                    form)
                {
                    IsUpsert = true,
                };

                writeModels.Add(model);
            }

            return _context.AnswerKeyOpticalForms.BulkWriteAsync(writeModels);
        }

        public async Task<IReadOnlyCollection<AnswerKeyOpticalForm>> GetByExamIdAsync(int examId)
        {
            var list = await _context.AnswerKeyOpticalForms
                .Find(Builders<AnswerKeyOpticalForm>.Filter.Eq(x => x.ExamId, examId))
                .ToListAsync();

            foreach (var item in list)
            {
                item.Sections = item.Sections.OrderBy(s => s.FormPart)
                    .ThenBy(s => s.ListOrder)
                    .ToList();
            }

            return list;
        }

        public Task DeleteByExamIdAsync(int examId)
        {
            var aFilter = Builders<AnswerKeyOpticalForm>.Filter.Eq(x => x.ExamId, examId);
            return _context.AnswerKeyOpticalForms.DeleteManyAsync(aFilter);
        }

        public Task UpdateLessonNameAsync(int lessonId, string newLessonName)
        {
            var filter = Builders<AnswerKeyOpticalForm>.Filter
                .ElemMatch(x => x.Sections, s => s.LessonId == lessonId);
            var aUpdate = Builders<AnswerKeyOpticalForm>.Update
                .Set("Sections.$.LessonName", newLessonName);
            return _context.AnswerKeyOpticalForms.UpdateManyAsync(filter, aUpdate);
        }

        public async Task UpdateSubjectNameAsync(int subjectId, string newSubjectName)
        {
            var filter = Builders<AnswerKeyOpticalForm>.Filter.Eq(
                "Sections.Answers.SubjectId", subjectId);

            using var cursor = await _context.AnswerKeyOpticalForms.FindAsync(filter);
            await cursor.ForEachAsync(form =>
            {
                foreach (var answer in form.Sections.SelectMany(s => s.Answers)
                    .Where(a => a.SubjectId == subjectId))
                {
                    answer.SubjectName = newSubjectName;
                }

                return _context.AnswerKeyOpticalForms.ReplaceOneAsync(f => f.Id == form.Id, form);
            });
        }
    }
}