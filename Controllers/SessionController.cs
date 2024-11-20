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
        public async Task<IActionResult> CreateSession()
        {
            try
            {
                var request = new SessionRequestBuilder()
                    .WithType(1)
                    .WithConnection("165.114.41.21", "5117")
                    .WithBrowser("Chrome", "47.0.0.12") 
                    .Build();

                var response = await _apiService.PostAsync<CreateSessionResponse>(request);
                var serializedconnection = JsonConvert.SerializeObject(response);

                Console.WriteLine("REEEEESPONSE " + serializedconnection);
                if (response != null && response.Status == "Success" && response.Data != null)
                {
                    HttpContext.Session.SetString("session-id", response.Data.SessionId);
                    HttpContext.Session.SetString("device-id", response.Data.DeviceId);

                    return ResponseUtil.Success(response.Data);
                }

                return ResponseUtil.Error("Failed to create session.");
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error($"Internal server error: {ex.Message}");
            }
        }
    }
}
