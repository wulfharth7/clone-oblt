using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult SetSessionInfo(SessionResponse response)
        {
            if (response != null && response.Status == "Success" && response.Data != null)
            {
                _httpContextAccessor.HttpContext?.Session.SetString("session-id", response.Data.SessionId); // We have to save these session-id and device-id values.
                _httpContextAccessor.HttpContext?.Session.SetString("device-id", response.Data.DeviceId);   // So we can use them application-wide for the user's requests.
                                                                                                            // This function helps us to do that.
                return ResponseUtil.Success(response.Data);
            }

            return ResponseUtil.Error("Failed to create session.");
        }
    }
}
