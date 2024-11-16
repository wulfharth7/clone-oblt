using clone_oblt.Models;
using clone_oblt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace clone_oblt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly IObiletApiService _apiService;

        public SessionController(IObiletApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSession()
        {
            try
            {
                var request = new CreateSessionRequest
                {
                    Type = 1,
                    Connection = new Models.ConnectionInfo
                    {
                        IpAddress = "165.114.41.21", // this will be changed and be dynamic in the future
                                                     // at the beginning my first aim was to make the session creation work so I can implement
                                                     // other api endpoints
                        Port = "5117"
                    },
                    Browser = new BrowserInfo       // these also will be refactored and be dynamic
                    {
                        Name = "Chrome",
                        Version = "47.0.0.12"
                    }
                };

                var response = await _apiService.PostAsync<CreateSessionResponse>(request);
                Console.WriteLine($"Response from API Service: {JsonSerializer.Serialize(response)}");

                if (response != null && response.Status == "Success" && response.Data != null)
                {
                    return Ok(new
                    {
                        status = "Success",
                        data = response.Data
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        status = "Error",
                        message = "Failed to create session."
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while creating session: {ex.Message}");

                return StatusCode(500, new
                {
                    status = "Error",
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }
    }
}
