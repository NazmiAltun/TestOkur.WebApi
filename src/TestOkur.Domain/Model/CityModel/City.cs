namespace TestOkur.Domain.Model.CityModel
{
    using System.Collections.Generic;
    using TestOkur.Domain.SeedWork;

    public class City : Entity
    {
        private readonly List<District> _districts;

        public City(long id, Name name)
            : this()
        {
            Id = id;
            Name = name;
        }

        protected City()
        {
            _districts = new List<District>();
        }

        public Name Name { get; private set; }

        public IEnumerable<District> Districts => _districts.AsReadOnly();

        public void AddDistrict(long id, string name)
        {
            _districts.Add(new District(id, name));
        }
    }
}
