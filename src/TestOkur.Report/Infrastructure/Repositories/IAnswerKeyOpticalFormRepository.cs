namespace TestOkur.Report.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;

    public interface IAnswerKeyOpticalFormRepository
    {
        Task AddOrUpdateManyAsync(IEnumerable<AnswerKeyOpticalForm> forms);

        Task<IReadOnlyCollection<AnswerKeyOpticalForm>> GetByExamIdAsync(int examId);

        Task DeleteByExamIdAsync(int examId);

        Task UpdateLessonNameAsync(int lessonId, string newLessonName);

        Task UpdateSubjectNameAsync(int subjectId, string newSubjectName);
    }
}