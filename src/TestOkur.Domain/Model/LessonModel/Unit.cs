namespace TestOkur.Domain.Model.LessonModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Domain.SeedWork;

    public class Unit : Entity, IAuditable
    {
        private readonly List<Subject> _subjects;

        public Unit(Name name, Lesson lesson, Grade grade, bool shared)
            : this()
        {
            Name = name;
            Grade = grade;
            Shared = shared;
            Lesson = lesson ?? throw new ArgumentNullException(nameof(lesson));
        }

        protected Unit()
        {
            _subjects = new List<Subject>();
        }

        public Name Name { get; private set; }

        public Lesson Lesson { get; private set; }

        public Grade Grade { get; private set; }

        public bool Shared { get; private set; }

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

            throw DomainException.With("Subject id {0} does not exist in {1} - {2} unit", id, Id, Name);
        }

        public void AddSubject(Name subjectName) => AddSubject(subjectName, false);

        public void AddSubject(Name subjectName, bool shared)
        {
            if (Subjects.Any(s => s.Name == subjectName))
            {
                throw DomainException.With($"SubjectName '{subjectName}' already exists in this unit.");
            }

            _subjects.Add(new Subject(subjectName, shared));
        }
    }
}
