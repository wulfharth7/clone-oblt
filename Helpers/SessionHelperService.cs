using clone_oblt.Helpers.HelperInterfaces;

namespace clone_oblt.Helpers
{
    //Helps to gather current user's session and device id.
    //Current Session Service is an api service and only works for creating the session api.
    //Code was repeating itself, hence I refactored it into a helper class and now we can get session/device id with one line of code in other api services.

    public class SessionHelperService : ISessionHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public (string SessionId, string DeviceId) GetSessionInfo()
        {
            var sessionId = _httpContextAccessor.HttpContext?.Session.GetString("session-id");
            var deviceId = _httpContextAccessor.HttpContext?.Session.GetString("device-id");

            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                throw new InvalidOperationException("Session ID or Device ID is missing.");
            }

            return (sessionId, deviceId);
        }
    }
}
