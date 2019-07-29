namespace TestOkur.Report.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using TestOkur.Contracts.Student;
	using TestOkur.Optic.Form;

	public interface IOpticalFormRepository
	{
		Task AddOrUpdateManyAsync(IEnumerable<StudentOpticalForm> forms);

		Task AddManyAsync(IEnumerable<StudentOpticalForm> forms);

		Task AddManyAsync(IEnumerable<AnswerKeyOpticalForm> forms);

		Task<IEnumerable<StudentOpticalForm>> GetStudentOpticalFormAsync(int examId);

		Task<IEnumerable<AnswerKeyOpticalForm>> GetAnswerKeyOpticalForms(int examId);

		Task<IEnumerable<int>> GetExamIdsAsync(
			Expression<Func<StudentOpticalForm, int>> selector,
			int selectorId);

		Task DeleteByExamIdAsync(int examId);

		Task DeleteByClassroomIdAsync(int classroomId);

		Task DeleteByStudentIdAsync(int studentId);

		Task DeleteAnswerKeyOpticalFormsByExamIdAsync(int examId);

		Task DeleteManyAsync(IEnumerable<StudentOpticalForm> forms);

		Task<StudentOpticalForm> DeleteOneAsync(string id);

		Task UpdateStudentAsync(IStudentUpdated studentUpdatedEvent);

		Task UpdateClassroomAsync(int classroomId, int grade, string name);

		Task UpdateLessonNameAsync(int lessonId, string newLessonName);

		Task UpdateSubjectNameAsync(int subjectId, string newSubjectName);
	}
}
