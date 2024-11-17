using clone_oblt.Builders;
using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;

namespace clone_oblt.Services
{   
    public class JourneysApiService : ApiServiceBase, IJourneysApiService
    {
        private readonly ISessionHelperService _sessionHelperService;

        public JourneysApiService(HttpClient httpClient, IConfiguration configuration, ISessionHelperService sessionHelperService)
            : base(httpClient, configuration, "ApiSettings:JourneysApiUrl")
        {
            _sessionHelperService = sessionHelperService;
        }

        public async Task<List<JourneyDetails>> GetJourneysAsync(JourneyRequest journeyRequest)
        {
            var (sessionId, deviceId) = _sessionHelperService.GetSessionInfo();
            Console.WriteLine("[TEST TEST TEST ]"+ sessionId);

            var request = new JourneyRequestBuilder()
                .WithDeviceSession(sessionId, deviceId)
                .WithLanguage(journeyRequest.Language ?? "tr-TR")
                .WithDate(journeyRequest.Data.DepartureDate)
                .WithJourneyData(journeyRequest.Data.OriginId, journeyRequest.Data.DestinationId, journeyRequest.Data.DepartureDate)
                .Build();

            var journeyResponse = await SendRequestAsync<JourneyRequest, JourneyResponse>(request, _apiUrl);
            return SortJourneysByDepartureDateAndTime(journeyResponse);
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
