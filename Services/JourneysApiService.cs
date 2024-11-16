using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using clone_oblt.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace clone_oblt.Services
{
    public class JourneysApiService : IJourneysApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _journeysApiUrl;
        private readonly string _apiKey;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JourneysApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _journeysApiUrl = configuration["ApiSettings:JourneysApiUrl"];
            _apiKey = SingletonApiKey.GetInstance().ApiKey;
            _httpContextAccessor = httpContextAccessor;
        }
        //  TODO
        //  project requirements about default settings will be done later.
        public async Task<List<JourneyDetails>> GetJourneysAsync(JourneyRequest journeyRequest)
        {
            var originId = journeyRequest.Data?.OriginId ?? 349; 
            var destinationId = journeyRequest.Data?.DestinationId ?? 356;
            var departureDate = journeyRequest.Data?.DepartureDate ?? new DateTime(2024, 12, 1);
            var date = departureDate;
            var language = journeyRequest.Language ?? "tr-TR";

            var sessionId = _httpContextAccessor.HttpContext.Session.GetString("session-id");
            var deviceId = _httpContextAccessor.HttpContext.Session.GetString("device-id");

            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                throw new InvalidOperationException("Session ID or Device ID is missing.");
            }

            var request = new JourneyRequest
            {
                DeviceSession = new DeviceSession
                {
                    SessionId = sessionId,
                    DeviceId = deviceId
                },
                Date = date,
                Language = language,
                Data = new JourneyData
                {
                    OriginId = originId,
                    DestinationId = destinationId,
                    DepartureDate = departureDate
                }
            };

            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd"
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request, jsonSettings), Encoding.UTF8, "application/json");

            var serializedContent = await jsonContent.ReadAsStringAsync();
            Console.WriteLine("[Serialized Request]: " + serializedContent);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _journeysApiUrl)
            {
                Content = jsonContent
            };

            AddHeadersToRequest(requestMessage);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var journeysResponse = JsonConvert.DeserializeObject<JourneyResponse>(responseContent);
                return journeysResponse?.Data;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Request failed: {ex.Message}");
            }
        }


        private void AddHeadersToRequest(HttpRequestMessage requestMessage)
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