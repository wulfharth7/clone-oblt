using clone_oblt.Builders.Interfaces;
using clone_oblt.Models;

namespace clone_oblt.Builders
{
    //There are lots of request/response models and other DTOs in this project.
    //At the beginning they were getting created manually and I was just calling the class with a new keyword.

    //For this small amount of models, its fine. But what if the system keeps growing or DTOs have to change, get a new field and other stuff?
    //This is where we use the builder design pattern. It helps to create the DTOs and use them very efficiently, its scalable, readable.
    //And gives us a real easyness, if we want to change them in the future.

    //Hence, I've implemented Builder DP.
    public class JourneyRequestBuilder : IRequestBuilder<JourneyRequest>
    {
        private DeviceSession _deviceSession = new DeviceSession();
        private DateTime? _date;
        private string? _language;
        private JourneyData? _journeyData;

        public JourneyRequestBuilder WithDeviceSession(string sessionId, string deviceId)
        {
            _deviceSession.SessionId = sessionId;
            _deviceSession.DeviceId = deviceId;
            return this;
        }

        public JourneyRequestBuilder WithDate(DateTime? date)
        {
            _date = date;
            return this;
        }

        public JourneyRequestBuilder WithLanguage(string? language)
        {
            _language = language;
            return this;
        }

        public JourneyRequestBuilder WithJourneyData(int? originId, int? destinationId, DateTime? departureDate)
        {
            _journeyData = new JourneyData
            {
                OriginId = originId,
                DestinationId = destinationId,
                DepartureDate = departureDate
            };
            return this;
        }

        public JourneyRequest Build()
        {
            if (_journeyData == null) throw new InvalidOperationException("JourneyData is required for JourneyRequest.");

            return new JourneyRequest
            {
                DeviceSession = _deviceSession,
                Date = _date,
                Language = _language,
                Data = _journeyData
            };
        }
    }

}
