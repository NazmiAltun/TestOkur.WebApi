namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class EditContactCommand : CreateContactCommand
	{
		public EditContactCommand(Guid id, string firstName, string lastName, string phone, int contactType, string labels, int contactId)
			: base(id, firstName, lastName, phone, contactType, labels)
		{
			ContactId = contactId;
		}

		[DataMember]
		public int ContactId { get; private set; }
	}
}
