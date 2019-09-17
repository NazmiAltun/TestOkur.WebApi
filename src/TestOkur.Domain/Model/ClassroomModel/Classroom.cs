namespace TestOkur.Domain.Model.ClassroomModel
{
    using TestOkur.Domain.SeedWork;

    public class Classroom : Entity, IAuditable
    {
        public Classroom(Grade grade, Name name)
        {
            Grade = grade;
            Name = name;
        }

        protected Classroom()
        {
        }

        public Grade Grade { get; private set; }

        public Name Name { get; private set; }

        public void Update(Grade grade, Name name)
        {
            Grade = grade;
            Name = name;
        }
    }
}
