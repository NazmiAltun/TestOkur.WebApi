namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
    public sealed class AddSubjectCommand : CommandBase, IClearCache
	{
		public AddSubjectCommand(Guid id, string name, int unitId)
		 : base(id)
		{
			Name = name;
			UnitId = unitId;
		}

		public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public int UnitId { get; set; }
	}
}
