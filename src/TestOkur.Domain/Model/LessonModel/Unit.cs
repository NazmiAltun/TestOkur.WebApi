namespace TestOkur.Domain.Model.LessonModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Domain.SeedWork;

    public class Unit : Entity, IAuditable
	{
		private readonly List<Subject> _subjects;

		public Unit(Name name, Lesson lesson, Grade grade)
			: this()
		{
			Name = name;
			Grade = grade;
			Lesson = lesson ?? throw new ArgumentNullException(nameof(lesson));
		}

		protected Unit()
		{
			_subjects = new List<Subject>();
		}

		public Name Name { get; private set; }

		public Lesson Lesson { get; private set; }

		public Grade Grade { get; private set; }

		public IReadOnlyCollection<Subject> Subjects => _subjects.AsReadOnly();

		public void SetName(Name name) => Name = name;

		public Subject RemoveSubject(int id)
		{
			for (var i = 0; i < _subjects.Count; i++)
			{
				if (_subjects[i].Id == id)
				{
					var subject = _subjects[i];
					_subjects.Remove(subject);

					return subject;
				}
			}

			return null;
		}

		public void AddSubject(Name subjectName)
		{
			if (Subjects.Any(s => s.Name == subjectName))
			{
				throw DomainException.With($"SubjectName '{subjectName}' already exists in this unit.");
			}

			_subjects.Add(new Subject(subjectName));
		}
	}
}
