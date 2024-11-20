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
    public class BusLocationBuilder : IRequestBuilder<BusLocationRequest>
    {
        private DeviceSession _deviceSession = new DeviceSession();
        private DateTime? _date;
        private string? _language;
        private string? _busLocationData;

        public BusLocationBuilder WithDeviceSession(string sessionId, string deviceId)
        {
            _deviceSession.SessionId = sessionId;
            _deviceSession.DeviceId = deviceId;
            return this;
        }

        public BusLocationBuilder WithDate(DateTime? date)
        {
            _date = date;
            return this;
        }

        public BusLocationBuilder WithLanguage(string? language)
        {
            _language = language;
            return this;
        }

        public BusLocationBuilder WithBusLocationData(string? data)
        {
            _busLocationData = data;
            return this;
        }

        public BusLocationRequest Build()
        {
            return new BusLocationRequest
            {
                DeviceSession = _deviceSession,
                Date = _date,
                Language = _language,
                Data = _busLocationData
            };
        }
    }

}
