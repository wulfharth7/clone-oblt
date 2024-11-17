using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace clone_oblt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller
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
                var request = new CreateSessionRequest
                {
                    Type = 1,
                    Connection = new Models.ConnectionInfo
                    {
                        IpAddress = "165.114.41.21", // this will be dynamic in the future
                        Port = "5117"
                    },
                    Browser = new BrowserInfo       // these also will be dynamic
                    {
                        Name = "Chrome",
                        Version = "47.0.0.12"
                    }
                };

                var response = await _apiService.PostAsync<CreateSessionResponse>(request);

                if (response != null && response.Status == "Success" && response.Data != null)
                {
                    HttpContext.Session.SetString("session-id", response.Data.SessionId);
                    HttpContext.Session.SetString("device-id", response.Data.DeviceId);

                    
                    return Ok(new
                    {
                        status = "Success",
                        data = response.Data
                    });
                }
                else
                {
                    HttpContext.Session.SetString("session-id", "PqtdftjloK3Kpka97+ILDzMa6D9740nggLiTzXiLlzA=");
                    HttpContext.Session.SetString("device-id", "PqtdftjloK3Kpka97+ILDzMa6D9740nggLiTzXiLlzA=");
                    var responsee = new CreateSessionResponse
                    {
                        Data = new SessionData
                        {
                            SessionId = "+Eqe+WzrJSYolfN+ulXchQQ0qhQEG5gqfBMqmwwuetQ=",
                            DeviceId = "PqtdftjloK3Kpka97+ILDzMa6D9740nggLiTzXiLlzA="
                        }
                    };
                    return Ok(new
                    {
                        status = "Success",
                        data = responsee.Data
                    });
                    //this will get back to working once 429 stops blocking me xd 
                    /*return BadRequest(new
                    {
                        status = "Error",
                        message = "Failed to create session."
                    });*/
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("session-id", "+Eqe+WzrJSYolfN+ulXchQQ0qhQEG5gqfBMqmwwuetQ=");
                HttpContext.Session.SetString("device-id", "VfH1N/wv59/yjYnlpCpcmQUCzoSAPQDxRiDjWENrXrg=");
                var response = new CreateSessionResponse{
                    Data= new SessionData
                    {
                        SessionId= "+Eqe+WzrJSYolfN+ulXchQQ0qhQEG5gqfBMqmwwuetQ=",
                        DeviceId= "VfH1N/wv59/yjYnlpCpcmQUCzoSAPQDxRiDjWENrXrg="
                    }
                };
                return Ok(new
                {
                    status = "Success",
                    data = response.Data
                });
                //will get back to normal after 429

                /*return StatusCode(500, new
                {
                    status = "Error",
                    message = $"Internal server error: {ex.Message}"
                });*/
            }
        }
    }
}
