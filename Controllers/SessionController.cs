using clone_oblt.Builders;
using clone_oblt.Builders.Interfaces;
using clone_oblt.Helpers;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ConnectionInfo = clone_oblt.Models.ConnectionInfo;

namespace clone_oblt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionApiService _apiService;

        public SessionController(ISessionApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSession([FromBody] SessionRequest request)
        {
            try
            {
                var response = await _apiService.CreateSessionAsync(request);

                /*if (response != null && response.Status == "Success" && response.Data != null)
                {*/
                    //For developing, sometimes i get 429 xd
                    HttpContext.Session.SetString("session-id", "ZZqcvpmKTFWk5fJc+KvYZbEtr4UmSDX7fGvZuB59OIs=" /*response.Data.SessionId*/);
                    HttpContext.Session.SetString("device-id", "OW/JcNRcQ2DoYjxxgUvi9plYf0cPnSCWbSFHObH6+aQ="/* response.Data.DeviceId*/);
               
                    return ResponseUtil.Success(response.Data);
                /*}

                return ResponseUtil.Error("Failed to create session.");*/
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error($"Internal server error: {ex.Message}");
            }
        }
    }
}
