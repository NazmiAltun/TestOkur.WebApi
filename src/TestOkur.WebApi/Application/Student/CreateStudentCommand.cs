namespace TestOkur.WebApi.Application.Student
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Contact;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;
    using Student = TestOkur.Domain.Model.StudentModel.Student;

    public class CreateStudentCommand : CommandBase, IClearCache
    {
        public CreateStudentCommand(
            Guid id,
            string firstName,
            string lastName,
            int studentNumber,
            int classroomId,
            string notes,
            string source,
            string citizenshipIdentity,
            IEnumerable<CreateContactCommand> contacts)
         : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            StudentNumber = studentNumber;
            Notes = notes;
            Source = source;
            CitizenshipIdentity = citizenshipIdentity;
            ClassroomId = classroomId;
            Contacts = contacts;
        }

        public CreateStudentCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int StudentNumber { get; set; }

        public int ClassroomId { get; set; }

        public string Notes { get; set; }

        public string Source { get; set; }

        public string CitizenshipIdentity { get; set; }

        public IEnumerable<CreateContactCommand> Contacts { get; set; }

        public Student ToDomainModel(Classroom classroom)
        {
            return new Student(
                FirstName,
                LastName,
                StudentNumber,
                classroom,
                Contacts?.Select(c => c.ToDomainModel()).Where(x => x != null) ?? Enumerable.Empty<Domain.Model.StudentModel.Contact>(),
                CitizenshipIdentity,
                Notes,
                Source);
        }
    }
}
