namespace TestOkur.Report.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using TestOkur.Contracts.Student;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Configuration;

    public class StudentOpticalFormRepository : IStudentOpticalFormRepository
    {
        private readonly TestOkurContext _context;

        public StudentOpticalFormRepository(ReportConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public async Task AddOrUpdateManyAsync(IEnumerable<StudentOpticalForm> forms)
        {
            if (forms == null || !forms.Any())
            {
                return;
            }

            var writeModels = new List<WriteModel<StudentOpticalForm>>();

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

            await _context.StudentOpticalForms.BulkWriteAsync(writeModels);
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

        public async Task UpdateSubjectNameAsync(int subjectId, string newSubjectName)
        {
            await UpdateStudentOpticalFormsSubject(subjectId, newSubjectName);
        }

        public async Task UpdateStudentAsync(IStudentUpdated studentUpdatedEvent)
        {
            var filter = Builders<StudentOpticalForm>.Filter
                .Eq(x => x.StudentId, studentUpdatedEvent.StudentId);
            var update = Builders<StudentOpticalForm>.Update
                .Set(x => x.ClassroomId, studentUpdatedEvent.ClassroomId)
                .Set(x => x.StudentFirstName, studentUpdatedEvent.FirstName)
                .Set(x => x.StudentLastName, studentUpdatedEvent.LastName)
                .Set(x => x.StudentNumber, studentUpdatedEvent.StudentNumber);
            await _context.StudentOpticalForms.UpdateManyAsync(filter, update);
        }

        public async Task UpdateClassroomAsync(int classroomId, int grade, string name)
        {
            var filter = Builders<StudentOpticalForm>.Filter
                .Eq(x => x.ClassroomId, classroomId);
            var update = Builders<StudentOpticalForm>.Update
                .Set(x => x.Classroom, $"{grade}/{name}");

            await _context.StudentOpticalForms.UpdateManyAsync(filter, update);
        }

        public async Task UpdateLessonNameAsync(int lessonId, string newLessonName)
        {
            var filter = Builders<StudentOpticalForm>.Filter
                    .ElemMatch(x => x.Sections, s => s.LessonId == lessonId);
            var update = Builders<StudentOpticalForm>.Update
                .Set("Sections.$.LessonName", newLessonName);
            await _context.StudentOpticalForms.UpdateManyAsync(filter, update);
        }

        public async Task AddManyAsync(IEnumerable<StudentOpticalForm> forms)
        {
            await _context.StudentOpticalForms.InsertManyAsync(forms);
        }

        public async Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalByStudentIdAsync(int studentId)
        {
            var list = await _context.StudentOpticalForms
                .Find(Builders<StudentOpticalForm>.Filter.Eq(x => x.StudentId, studentId))
                .ToListAsync();

            foreach (var item in list)
            {
                item.Sections = item.Sections.OrderBy(s => s.FormPart)
                    .ThenBy(s => s.ListOrder)
                    .ToList();
            }

            return list;
        }

        public async Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalFormsByExamIdAsync(int examId)
        {
            var list = await _context.StudentOpticalForms
                .Find(Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, examId))
                .ToListAsync();

            foreach (var item in list)
            {
                item.Sections = item.Sections.OrderBy(s => s.FormPart)
                    .ThenBy(s => s.ListOrder)
                    .ToList();
            }

            return list;
        }

        public async Task DeleteByStudentIdAsync(int studentId)
        {
            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.StudentId, studentId);
            await _context.StudentOpticalForms.DeleteManyAsync(filter);
        }

        public async Task DeleteByClassroomIdAsync(int classroomId)
        {
            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.ClassroomId, classroomId);
            await _context.StudentOpticalForms.DeleteManyAsync(filter);
        }

        public async Task DeleteByExamIdAsync(int examId)
        {
            await DeleteStudentOpticalFormsByExamIdAsync(examId);
        }

        public async Task DeleteManyAsync(IEnumerable<StudentOpticalForm> forms)
        {
            if (forms == null || !forms.Any())
            {
                return;
            }

            var filter = Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, forms.First().ExamId);
            filter &= Builders<StudentOpticalForm>.Filter.In(x => x.StudentId, forms.Select(x => x.StudentId));
            await _context.StudentOpticalForms.DeleteManyAsync(filter);
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

            using (var cursor = await _context.StudentOpticalForms.FindAsync(filter))
            {
                await cursor.ForEachAsync(form =>
                {
                    foreach (var answer in form.Sections.SelectMany(s => s.Answers)
                        .Where(a => a.SubjectId == subjectId))
                    {
                        answer.SubjectName = newSubjectName;
                    }

                    _context.StudentOpticalForms.ReplaceOneAsync(f => f.Id == form.Id, form);
                });
            }
        }

        private async Task DeleteStudentOpticalFormsByExamIdAsync(int examId)
        {
            var sFilter = Builders<StudentOpticalForm>.Filter.Eq(x => x.ExamId, examId);
            await _context.StudentOpticalForms.DeleteManyAsync(sFilter);
        }
    }
}
