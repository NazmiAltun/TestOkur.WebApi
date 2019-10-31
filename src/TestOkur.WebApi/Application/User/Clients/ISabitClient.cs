namespace TestOkur.WebApi.Application.User.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISabitClient
    {
        Task<IEnumerable<LicenseType>> GetLicenseTypesAsync();

        Task<IEnumerable<City>> GetCitiesAsync();
    }
}
