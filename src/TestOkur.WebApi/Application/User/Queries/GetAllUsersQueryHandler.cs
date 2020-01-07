namespace TestOkur.WebApi.Application.User.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Clients;
    using TestOkur.WebApi.Configuration;

    public sealed class GetAllUsersQueryHandler : QueryHandlerAsync<GetAllUsersQuery, IReadOnlyCollection<UserReadModel>>
    {
        private const string CityCacheKey = "CityCacheKey";

        private const string Sql = @"
								SELECT
								id,
								subject_id,
								sms_balance,
								city_id,
								district_id,
								email_value as email,
								phone_value as phone,
								first_name_value as first_name,
								last_name_value as last_name,
								school_name_value as school_name,
								notes,
								registrar_phone_value as registrar_phone,
								registrar_full_name_value as registrar_full_name,
                                referrer
								FROM users";

        private readonly ISabitClient _sabitClient;
        private readonly string _connectionString;
        private readonly ICacheManager<Dictionary<int, City>> _cityCacheManager;

        public GetAllUsersQueryHandler(
            ApplicationConfiguration configurationOptions,
            ISabitClient sabitClient,
            ICacheManager<Dictionary<int, City>> cityCacheManager)
        {
            _sabitClient = sabitClient;
            _cityCacheManager = cityCacheManager;
            _connectionString = configurationOptions.Postgres;
        }

        [ResultCaching(1)]
        public override async Task<IReadOnlyCollection<UserReadModel>> ExecuteAsync(
            GetAllUsersQuery query,
            CancellationToken cancellationToken = default)
        {
            var cityDict = await GetCityDictionaryAsync();

            await using var connection = new NpgsqlConnection(_connectionString);
            var userList = (await connection.QueryAsync<UserReadModel>(Sql)).ToList();

            foreach (var user in userList)
            {
                user.CityName = cityDict[user.CityId].Name;
                user.DistrictName = cityDict[user.CityId].Districts.First(d => d.Id == user.DistrictId).Name;
            }

            return userList;
        }

        private async Task<Dictionary<int, City>> GetCityDictionaryAsync()
        {
            var cityDict = _cityCacheManager.Get(CityCacheKey);

            if (cityDict != null)
            {
                return cityDict;
            }

            // TODO: A better dictionary approach
            cityDict = (await _sabitClient.GetCitiesAsync())
                .ToDictionary(x => x.Id, x => x);

            _cityCacheManager.Add(new CacheItem<Dictionary<int, City>>(
                CityCacheKey,
                cityDict,
                ExpirationMode.Absolute,
                TimeSpan.FromDays(30)));

            return cityDict;
        }
    }
}
