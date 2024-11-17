using clone_oblt.Builders;
using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using clone_oblt.Utils;
using Newtonsoft.Json;
using System.Text;

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

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(request, new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" }),
                Encoding.UTF8,
                "application/json"
            );

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _journeysApiUrl) { Content = jsonContent };


            HeaderUtils.AddHeadersToRequest(requestMessage, _apiKey);

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
    }
}
