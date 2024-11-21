using clone_oblt.Models;
using Microsoft.AspNetCore.Mvc;

namespace clone_oblt.Helpers.HelperInterfaces
{
    public interface ISessionHelperService
    {
        (string SessionId, string DeviceId) GetSessionInfo();
        IActionResult SetSessionInfo(SessionResponse response);
    }
}
