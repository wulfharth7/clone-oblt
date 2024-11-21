using clone_oblt.Builders;
using clone_oblt.Builders.Interfaces;
using clone_oblt.Helpers;
using clone_oblt.Helpers.HelperInterfaces;
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
        private readonly ISessionHelperService _sessionHelperService;

        public SessionController(ISessionApiService apiService, ISessionHelperService sessionHelperService)
        {
            _apiService = apiService;
            _sessionHelperService = sessionHelperService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSession([FromBody] SessionRequest request)
        {
            try
            {
                var response = await _apiService.CreateSessionAsync(request);
                return _sessionHelperService.SetSessionInfo(response);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error($"Internal server error: {ex.Message}");
            }
        }
    }
}
