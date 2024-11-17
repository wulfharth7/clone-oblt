using clone_oblt.Builders;
using clone_oblt.Helpers.HelperInterfaces;
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
        private readonly ISessionHelperService _sessionHelperService;

        public JourneysApiService(HttpClient httpClient, IConfiguration configuration, ISessionHelperService sessionHelperService)
        {
            _httpClient = httpClient;
            _journeysApiUrl = configuration["ApiSettings:JourneysApiUrl"];
            _apiKey = SingletonApiKey.GetInstance().ApiKey;
            _sessionHelperService = sessionHelperService;
        }

        public async Task<List<JourneyDetails>> GetJourneysAsync(JourneyRequest journeyRequest)
        {
            var (sessionId, deviceId) = _sessionHelperService.GetSessionInfo();

            var request = new JourneyRequestBuilder()
                .WithDeviceSession(sessionId, deviceId)
                .WithLanguage(journeyRequest.Language ?? "tr-TR")
                .WithDate(journeyRequest.Data.DepartureDate)
                .WithJourneyData(journeyRequest.Data.OriginId, journeyRequest.Data.DestinationId, journeyRequest.Data.DepartureDate)
                .Build();

            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd"
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request, jsonSettings), Encoding.UTF8, "application/json");

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
                return SortJourneysByDepartureDateAndTime(JsonConvert.DeserializeObject<JourneyResponse>(responseContent));
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Request failed: {ex.Message}");
            }
        }

        private List<JourneyDetails> SortJourneysByDepartureDateAndTime(JourneyResponse journeysResponse)
        {
            if (journeysResponse?.Data == null)
            {
                return new List<JourneyDetails>();
            }

            foreach (var journey in journeysResponse.Data)
            {
                if (journey?.Journey?.Stops != null)
                {
                    journey.Journey.Stops = journey.Journey.Stops
                        .Where(stop => stop?.Time.HasValue == true)
                        .OrderBy(stop => stop.Time)
                        .ToList();
                }
            }

            return journeysResponse.Data
                .Where(journey => journey?.Journey?.Departure.HasValue == true)
                .OrderBy(journey => journey.Journey.Departure)
                .ThenBy(journey => journey.Journey.Stops.FirstOrDefault()?.Time)
                .ToList();
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
