namespace TestOkur.Report.Infrastructure.Repositories
{
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Student;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Configuration;

    public class StudentOpticalFormRepository : IStudentOpticalFormRepository
    {
        private readonly TestOkurContext _context;
        private readonly ILogger<StudentOpticalFormRepository> _logger;

        public StudentOpticalFormRepository(ReportConfiguration configuration, ILogger<StudentOpticalFormRepository> logger)
        {
            _logger = logger;
            _context = new TestOkurContext(configuration);
        }

        public Task AddOrUpdateManyAsync(IEnumerable<StudentOpticalForm> forms)
        {
            if (forms == null || !forms.Any())
            {
                return Task.CompletedTask;
            }

            var writeModels = new List<WriteModel<StudentOpticalForm>>(forms.Count());

            foreach (var form in forms)
            {
                var model = new ReplaceOneModel<StudentOpticalForm>(
                    Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, form.ExamId) &
                    Builders<StudentOpticalForm>.Filter.Eq(x => x.StudentId, form.StudentId),
                    form)
                {
                    IsUpsert = true,
                };

                writeModels.Add(model);
            }

            return _context.StudentOpticalForms.BulkWriteAsync(writeModels);
        }

        public async Task<IEnumerable<int>> GetExamIdsAsync(
         Expression<Func<StudentOpticalForm, int>> selector,
         int selectorId)
        {
            var filter = Builders<StudentOpticalForm>.Filter.Eq(selector, selectorId);
            var examIdList = new List<int>();

            using (var cursor = await _context.StudentOpticalForms.FindAsync(filter))
            {
                await cursor.ForEachAsync(form =>
                {
                    examIdList.Add(form.ExamId);
                });
            }

            return examIdList.Distinct();
        }

        public Task UpdateSubjectNameAsync(int subjectId, string newSubjectName)
        {
            return UpdateStudentOpticalFormsSubject(subjectId, newSubjectName);
        }

        public Task UpdateStudentAsync(IStudentUpdated studentUpdatedEvent)
        {
            var filter = Builders<StudentOpticalForm>.Filter
                .Eq(x => x.StudentId, studentUpdatedEvent.StudentId);
            var update = Builders<StudentOpticalForm>.Update
                .Set(x => x.ClassroomId, studentUpdatedEvent.ClassroomId)
                .Set(x => x.StudentFirstName, studentUpdatedEvent.FirstName)
                .Set(x => x.StudentLastName, studentUpdatedEvent.LastName)
                .Set(x => x.StudentNumber, studentUpdatedEvent.StudentNumber);
            return _context.StudentOpticalForms.UpdateManyAsync(filter, update);
        }

        public Task UpdateClassroomAsync(int classroomId, int grade, string name)
        {
            var filter = Builders<StudentOpticalForm>.Filter
                .Eq(x => x.ClassroomId, classroomId);
            var update = Builders<StudentOpticalForm>.Update
                .Set(x => x.Classroom, $"{grade}/{name}");

            return _context.StudentOpticalForms.UpdateManyAsync(filter, update);
        }

        public Task UpdateLessonNameAsync(int lessonId, string newLessonName)
        {
            var filter = Builders<StudentOpticalForm>.Filter
                    .ElemMatch(x => x.Sections, s => s.LessonId == lessonId);
            var update = Builders<StudentOpticalForm>.Update
                .Set("Sections.$.LessonName", newLessonName);
            return _context.StudentOpticalForms.UpdateManyAsync(filter, update);
        }

        public Task AddManyAsync(IEnumerable<StudentOpticalForm> forms)
        {
            return _context.StudentOpticalForms.InsertManyAsync(forms);
        }

        public async Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalByStudentIdAsync(int studentId)
        {
            var sw = Stopwatch.StartNew();
            var list = await _context.StudentOpticalForms
                .Find(Builders<StudentOpticalForm>.Filter.Eq(x => x.StudentId, studentId))
                .ToListAsync();
            _logger.LogWarning($"Fetching student optical forms took {sw.ElapsedMilliseconds} ms");
            sw = Stopwatch.StartNew();
            foreach (var item in list)
            {
                item.Sections = item.Sections.OrderBy(s => s.FormPart)
                    .ThenBy(s => s.ListOrder)
                    .ToList();
            }

            _logger.LogWarning($"Re-ordering sections took {sw.ElapsedMilliseconds} ms");
            return list;
        }

        public async Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalFormsByExamIdAsync(int examId, string userId)
        {
            var sw = Stopwatch.StartNew();
            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, examId);
            filter &= Builders<StudentOpticalForm>.Filter.Eq(x => x.UserId, userId);

            var list = await _context.StudentOpticalForms
                .Find(filter)
                .ToListAsync();
            _logger.LogWarning($"Fetching student optical forms took {sw.ElapsedMilliseconds} ms");
            sw = Stopwatch.StartNew();

            foreach (var item in list)
            {
                item.Sections = item.Sections.OrderBy(s => s.FormPart)
                    .ThenBy(s => s.ListOrder)
                    .ToList();
            }

            _logger.LogWarning($"Re-ordering sections took {sw.ElapsedMilliseconds} ms");

            return list;
        }

        public async Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalFormsByExamIdAsync(int examId)
        {
            var sw = Stopwatch.StartNew();

            var list = await _context.StudentOpticalForms
                .Find(Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, examId))
                .ToListAsync();
            _logger.LogWarning($"Fetching student optical forms took {sw.ElapsedMilliseconds} ms");
            sw = Stopwatch.StartNew();

            foreach (var item in list)
            {
                item.Sections = item.Sections.OrderBy(s => s.FormPart)
                    .ThenBy(s => s.ListOrder)
                    .ToList();
            }

            _logger.LogWarning($"Re-ordering sections took {sw.ElapsedMilliseconds} ms");

            return list;
        }

        public Task DeleteByStudentIdAsync(int studentId)
        {
            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.StudentId, studentId);
            return _context.StudentOpticalForms.DeleteManyAsync(filter);
        }

        public Task DeleteByClassroomIdAsync(int classroomId)
        {
            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.ClassroomId, classroomId);
            return _context.StudentOpticalForms.DeleteManyAsync(filter);
        }

        public Task DeleteByExamIdAsync(int examId)
        {
            var sFilter = Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, examId);
            return _context.StudentOpticalForms.DeleteManyAsync(sFilter);
        }

        public Task DeleteManyAsync(IEnumerable<StudentOpticalForm> forms)
        {
            if (forms == null || !forms.Any())
            {
                return Task.CompletedTask;
            }

            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, forms.First().ExamId);
            filter &= Builders<StudentOpticalForm>.Filter.In(x => x.StudentId, forms.Select(x => x.StudentId));
            return _context.StudentOpticalForms.DeleteManyAsync(filter);
        }

        public Task<StudentOpticalForm> DeleteOneAsync(string id)
        {
            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.Id, id);
            return _context.StudentOpticalForms.FindOneAndDeleteAsync(filter);
        }

        private async Task UpdateStudentOpticalFormsSubject(int subjectId, string newSubjectName)
        {
            var filter = Builders<StudentOpticalForm>.Filter.Eq(
                "Sections.Answers.SubjectId", subjectId);

            using var cursor = await _context.StudentOpticalForms.FindAsync(filter);
            await cursor.ForEachAsync(form =>
            {
                foreach (var answer in form.Sections.SelectMany(s => s.Answers)
                    .Where(a => a.SubjectId == subjectId))
                {
                    answer.SubjectName = newSubjectName;
                }

                return _context.StudentOpticalForms.ReplaceOneAsync(f => f.Id == form.Id, form);
            });
        }
    }
}
