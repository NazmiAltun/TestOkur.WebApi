namespace TestOkur.WebApi.Application.Lesson.Commands
{
	using System.Collections.Generic;
	using TestOkur.Infrastructure.Cqrs;

	public class DeleteSubjectCommand : CommandBase, IClearCache
	{
		public DeleteSubjectCommand(int unitId, int subjectId)
		{
			UnitId = unitId;
			SubjectId = subjectId;
		}

		public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

		public int UnitId { get; }

		public int SubjectId { get; }
	}
}
