namespace TestOkur.Domain.Model
{
    using TestOkur.Domain.SeedWork;

    public class SchoolType : Enumeration
    {
        public static readonly SchoolType PrimaryAndSecondary = new SchoolType(1, "Primary And Secondary");
        public static readonly SchoolType High = new SchoolType(2, "High");

        public SchoolType(int id, string name)
            : base(id, name)
        {
        }

        protected SchoolType()
        {
        }
    }
}
