﻿namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using TestOkur.Common;
    using TestOkur.Notification.Models;

    public class ReportClient : IReportClient
    {
        private const string StatisticsEndpoint = "api/v1/report-requests";

        private readonly HttpClient _httpClient;
        private readonly IOAuthClient _oAuthClient;

        public ReportClient(
            HttpClient httpClient,
            IOAuthClient oAuthClient)
        {
            _httpClient = httpClient;
            _oAuthClient = oAuthClient;
        }

        public Task<ReportStatisticsModel> GetStatisticsAsync()
        {
            return GetAsync<ReportStatisticsModel>(StatisticsEndpoint);
        }

        private async Task<TModel> GetAsync<TModel>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TModel>(json, DefaultJsonSerializerSettings.Instance);
        }

        private async Task SetBearerToken()
        {
            _httpClient.SetBearerToken(await _oAuthClient.GetTokenAsync());
        }
    }
}
