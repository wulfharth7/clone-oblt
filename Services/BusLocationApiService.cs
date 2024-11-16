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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusLocationApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _busLocationApiUrl = configuration["ApiSettings:BusLocationsApiUrl"];
            _apiKey = SingletonApiKey.GetInstance().ApiKey;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BusLocationData>> GetBusLocationsAsync()
        {
            var sessionId = _httpContextAccessor.HttpContext.Session.GetString("session-id");
            var deviceId = _httpContextAccessor.HttpContext.Session.GetString("device-id");

            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                throw new InvalidOperationException("Session ID or Device ID is missing.");
            }

            var request = new BusLocationRequest
            {
                Data = "ova",                   //when i start developing the front-end side, this will change into a dynamic struct. right now
                                                //to see that it works, I went along with a static query.
                DeviceSession = new DeviceSession
                {
                    SessionId = sessionId,
                    DeviceId = deviceId
                },
                Date = DateTime.Now,
                Language = "tr-TR"
            };

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

                // Return BusLocationData directly, no need for anonymous type
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
