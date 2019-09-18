namespace TestOkur.WebApi.Application.Student
{
    using System.Collections.Generic;
    using TestOkur.WebApi.Application.Contact;

    public class StudentReadModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int StudentNumber { get; set; }

        public int ClassroomId { get; set; }

        public int ClassroomGrade { get; set; }

        public string ClassroomName { get; set; }

        public string Notes { get; set; }

        public string Source { get; set; }

        public List<ContactReadModel> Contacts { get; set; } = new List<ContactReadModel>();
    }
}
