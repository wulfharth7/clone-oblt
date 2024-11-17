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

            AddHeadersAndUrlToRequest(requestMessage);

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

        private void AddHeadersAndUrlToRequest(HttpRequestMessage requestMessage)
        {
            var headers = new
            {
                Authorization = $"Basic {_apiKey}",
                ContentType = "application/json"
            };

            var headerDictionary = ConvertHeadersToDictionary(headers);

            foreach (var header in headerDictionary)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
                Console.WriteLine($"Header: {header.Key} = {header.Value}");
            }
        }

        private Dictionary<string, string> ConvertHeadersToDictionary(object headers)
        {
            var headerDictionary = new Dictionary<string, string>();
            var properties = headers.GetType().GetProperties();
            foreach (var property in properties)
            {
                headerDictionary.Add(property.Name, property.GetValue(headers)?.ToString());
            }
            return headerDictionary;
        }
    }
}
