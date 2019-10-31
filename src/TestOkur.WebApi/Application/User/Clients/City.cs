namespace TestOkur.WebApi.Application.User.Clients
{
    using System.Collections.Generic;

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<District> Districts { get; set; } = new List<District>();
    }
}