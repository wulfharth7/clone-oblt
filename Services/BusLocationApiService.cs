using clone_oblt.Builders;
using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using clone_oblt.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace clone_oblt.Services
{
    public class BusLocationApiService : IBusLocationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _busLocationApiUrl;
        private readonly string _apiKey;
        private readonly ISessionHelperService _sessionHelperService;

        public BusLocationApiService(HttpClient httpClient, IConfiguration configuration, ISessionHelperService sessionHelperService)
        {
            _httpClient = httpClient;
            _busLocationApiUrl = configuration["ApiSettings:BusLocationsApiUrl"];
            _apiKey = SingletonApiKey.GetInstance().ApiKey;
            _sessionHelperService = sessionHelperService;
        }

        public async Task<List<BusLocationData>> GetBusLocationsAsync(BusLocationRequest requestbody)
        {
            var (sessionId, deviceId) = _sessionHelperService.GetSessionInfo();
            var request = new BusLocationRequestBuilder()
                .WithBusLocationData(requestbody.Data)
                .WithDeviceSession(sessionId, deviceId)
                .WithDate(DateTime.Now)
                .WithLanguage("tr-TR")
                .Build();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _busLocationApiUrl)
            {
                Content = jsonContent
            };

            HeaderUtils.AddHeadersToRequest(requestMessage, _apiKey);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var busLocationResponse = JsonConvert.DeserializeObject<BusLocationResponse>(responseContent);
                return busLocationResponse?.Data?.ToList();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
                throw new Exception($"Request failed with status code: {ex.Message}");
            }
        }
    }
}
