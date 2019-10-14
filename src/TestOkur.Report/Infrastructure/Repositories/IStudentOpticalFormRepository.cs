namespace TestOkur.Report.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Student;
    using TestOkur.Optic.Form;

    public interface IStudentOpticalFormRepository
    {
        Task AddOrUpdateManyAsync(IEnumerable<StudentOpticalForm> forms);

        Task AddManyAsync(IEnumerable<StudentOpticalForm> forms);

        Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalFormsByExamIdAsync(int examId);

        Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalByStudentIdAsync(int studentId);

        Task<IEnumerable<int>> GetExamIdsAsync(
            Expression<Func<StudentOpticalForm, int>> selector,
            int selectorId);

        Task DeleteByExamIdAsync(int examId);

        Task DeleteByClassroomIdAsync(int classroomId);

        Task DeleteByStudentIdAsync(int studentId);

        Task DeleteManyAsync(IEnumerable<StudentOpticalForm> forms);

        Task<StudentOpticalForm> DeleteOneAsync(string id);

        Task UpdateStudentAsync(IStudentUpdated studentUpdatedEvent);

        Task UpdateClassroomAsync(int classroomId, int grade, string name);

        Task UpdateLessonNameAsync(int lessonId, string newLessonName);

        Task UpdateSubjectNameAsync(int subjectId, string newSubjectName);
    }
}
