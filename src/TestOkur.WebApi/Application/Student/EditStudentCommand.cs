namespace TestOkur.WebApi.Application.Student
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Contact;

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
            string citizenshipIdentity,
            List<CreateContactCommand> contacts)
            : base(id)
        {
            StudentId = studentId;
            NewFirstName = newFirstName;
            NewLastName = newLastName;
            NewStudentNumber = newStudentNumber;
            NewClassroomId = newClassroomId;
            NewNotes = newNotes;
            CitizenshipIdentity = citizenshipIdentity;
            Contacts = contacts;
        }

        public EditStudentCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        public int StudentId { get; set; }

        public string NewFirstName { get; set; }

        public string NewLastName { get; set; }

        public int NewStudentNumber { get; set; }

        public int NewClassroomId { get; set; }

        public string NewNotes { get; set; }

        public string CitizenshipIdentity { get; set; }

        public List<CreateContactCommand> Contacts { get; set; }
    }
}
