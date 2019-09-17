namespace TestOkur.Domain.Model.StudentModel
{
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Domain.Model.ClassroomModel;
    using TestOkur.Domain.SeedWork;

    public class Student : Entity, IAuditable
	{
		private readonly List<Contact> _contacts = new List<Contact>();

		public Student(
			Name firstName,
			Name lastName,
			StudentNumber studentNumber,
			Classroom classroom,
			IEnumerable<Contact> contacts,
			string notes)
		{
			FirstName = firstName;
			LastName = lastName;
			StudentNumber = studentNumber;
			Classroom = classroom;
			_contacts.AddRange(contacts);
			Notes = notes;
		}

		protected Student()
		{
		}

		public Name FirstName { get; private set; }

		public Name LastName { get; private set; }

		public StudentNumber StudentNumber { get; private set; }

		public Classroom Classroom { get; private set; }

		public string Notes { get; private set; }

		public IEnumerable<Contact> Contacts => _contacts.AsReadOnly();

		public void Update(
			Name newFirstName,
			Name newLastName,
			StudentNumber studentNumber,
			Classroom newClassroom,
			IEnumerable<Contact> contacts,
			string notes)
		{
			FirstName = newFirstName;
			LastName = newLastName;
			StudentNumber = studentNumber;
			Classroom = newClassroom;
			Notes = notes;
			_contacts.Clear();

			if (contacts != null && contacts.Any())
			{
				_contacts.AddRange(contacts);
			}
		}
	}
}
