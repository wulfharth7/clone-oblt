using clone_oblt.Models;
using clone_oblt.Builders.Interfaces;

namespace clone_oblt.Builders
{
    //There are lots of request/response models and other DTOs in this project.
    //At the beginning they were getting created manually and I was just calling the class with a new keyword.

    //For this small amount of models, its fine. But what if the system keeps growing or DTOs have to change, get a new field and other stuff?
    //This is where we use the builder design pattern. It helps to create the DTOs and use them very efficiently, its scalable, readable.
    //And gives us a real easyness, if we want to change them in the future.

    //Hence, I've implemented Builder DP.
    public class CreateSessionResponseBuilder : IRequestBuilder<CreateSessionResponse>
    {
        private string? _sessionId;
        private string? _deviceId;

        public CreateSessionResponseBuilder WithSessionId(string sessionId)
        {
            _sessionId = sessionId;
            return this;
        }

        public CreateSessionResponseBuilder WithDeviceId(string deviceId)
        {
            _deviceId = deviceId;
            return this;
        }

        public CreateSessionResponse Build()
        {
            if (string.IsNullOrEmpty(_sessionId) || string.IsNullOrEmpty(_deviceId))
            {
                throw new InvalidOperationException("Both SessionId and DeviceId are required to build CreateSessionResponse.");
            }

            return new CreateSessionResponse
            {
                Data = new SessionData
                {
                    SessionId = _sessionId,
                    DeviceId = _deviceId
                }
            };
        }
    }
}
