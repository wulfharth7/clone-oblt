﻿using clone_oblt.Builders;
using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;

namespace clone_oblt.Services
{   
    //Api services in this project are our "proxies" to communicate with the api of obilet.
    //For those specific endpoints, we use these classes, send request over them using the builders and session ids.
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
            if (CheckSameDestination(journeyRequest))
            {
                throw new Exception("Destination and Origin locations can't be the same!");
            }
            else
            {
                return await GetJourneys(journeyRequest);
            }
        }

        private async Task<List<JourneyDetails>> GetJourneys(JourneyRequest journeyRequest)
        {
            var (sessionId, deviceId) = _sessionHelperService.GetSessionInfo(); //Brings the session info

            var request = new JourneyRequestBuilder()
                .WithDeviceSession(sessionId, deviceId)
                .WithLanguage(journeyRequest.Language ?? "tr-TR")
                .WithDate(journeyRequest.Data.DepartureDate)
                .WithJourneyData(journeyRequest.Data.OriginId, journeyRequest.Data.DestinationId, journeyRequest.Data.DepartureDate)
                .Build();

            var journeyResponse = await SendRequestAsync<JourneyRequest, JourneyResponse>(request, _apiUrl);
            if (journeyResponse != null)
                return SortJourneysByDepartureDateAndTime(journeyResponse); //It was asked that, we give the response sorted by date and time. This function does that.
            else
                throw new Exception();
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
        //The client actually checks if they are the same or not. 
        //But there could be such cases where someone tries to send direct requests to our endpoint,
        //Or a bug may happen and we dont want backend to let this situation run.
        private bool CheckSameDestination(JourneyRequest req)
        {
            if (req.Data.DestinationId == req.Data.OriginId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
