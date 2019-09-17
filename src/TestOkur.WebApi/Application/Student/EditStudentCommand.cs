namespace TestOkur.WebApi.Application.Student
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Contact;

    [DataContract]
    public class EditStudentCommand : CommandBase, IClearCache
    {
        public EditStudentCommand(
			Guid id,
			int studentId,
			string newFirstName,
			string newLastName,
			int newStudentNumber,
			int newClassroomId,
			string newNotes,
			List<CreateContactCommand> contacts)
	        : base(id)
        {
            StudentId = studentId;
            NewFirstName = newFirstName;
            NewLastName = newLastName;
            NewStudentNumber = newStudentNumber;
            NewClassroomId = newClassroomId;
            NewNotes = newNotes;
            Contacts = contacts;
        }

        public IEnumerable<string> CacheKeys => new[]
        {
	        $"Students_{UserId}",
	        $"Contacts_{UserId}",
        };

        [DataMember]
        public int StudentId { get; private set; }

        [DataMember]
        public string NewFirstName { get; private set; }

        [DataMember]
        public string NewLastName { get; private set; }

        [DataMember]
        public int NewStudentNumber { get; private set; }

        [DataMember]
        public int NewClassroomId { get; private set; }

        [DataMember]
        public string NewNotes { get; private set; }

        [DataMember]
        public List<CreateContactCommand> Contacts { get; private set; }
    }
}
