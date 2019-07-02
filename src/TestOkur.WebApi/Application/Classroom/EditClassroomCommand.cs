namespace TestOkur.WebApi.Application.Classroom
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using TestOkur.Infrastructure.Cqrs;

	[DataContract]
	public sealed class EditClassroomCommand : CommandBase, IClearCache
	{
		public EditClassroomCommand(Guid id, int classroomId, string newName, int newGrade)
			: base(id)
		{
			ClassroomId = classroomId;
			NewName = newName;
			NewGrade = newGrade;
		}

		public IEnumerable<string> CacheKeys => new[] { $"Classrooms_{UserId}" };

		[DataMember]
		public int ClassroomId { get; private set; }

		[DataMember]
		public string NewName { get; private set; }

		[DataMember]
		public int NewGrade { get; private set; }
	}
}
