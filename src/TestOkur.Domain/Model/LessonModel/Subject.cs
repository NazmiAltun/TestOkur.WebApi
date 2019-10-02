namespace TestOkur.Domain.Model.LessonModel
{
    using TestOkur.Domain.SeedWork;

    public class Subject : Entity, IAuditable
    {
        public Subject(Name name, bool shared)
        {
            Name = name;
            Shared = shared;
        }

        protected Subject()
        {
        }

        public bool Shared { get; private set; }

        public Name Name { get; private set; }

        public void SetName(string name)
        {
            Name = name;
        }
    }
}
