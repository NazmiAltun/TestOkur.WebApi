namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
    public class BulkEditContactsCommand : CommandBase, IClearCache
	{
		public BulkEditContactsCommand(Guid id, IEnumerable<EditContactCommand> commands)
			: base(id)
		{
			Commands = commands;
		}

		public IEnumerable<string> CacheKeys => new[]
		{
			$"Students_{UserId}",
			$"Contacts_{UserId}",
		};

		[DataMember]
		public IEnumerable<EditContactCommand> Commands { get; private set; }
	}
}
