namespace TestOkur.Domain.Model.CityModel
{
	using TestOkur.Domain.SeedWork;

	public class District : Entity
    {
        public District(long id, Name name)
        {
            Id = id;
            Name = name;
        }

        protected District()
        {
        }

        public Name Name { get; private set; }
    }
}
