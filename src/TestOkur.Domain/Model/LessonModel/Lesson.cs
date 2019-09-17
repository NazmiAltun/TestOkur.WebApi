namespace TestOkur.Domain.Model.LessonModel
{
    using TestOkur.Domain.SeedWork;

    public class Lesson : Entity, IAuditable
    {
        public Lesson(Name name)
        {
            Name = name;
        }

        protected Lesson()
        {
        }

        public Name Name { get; private set; }

        public void SetName(Name name)
        {
            Name = name;
        }
    }
}
