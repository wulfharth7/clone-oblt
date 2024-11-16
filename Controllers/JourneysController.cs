using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace clone_oblt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : Controller
    {
        private readonly IJourneysApiService _journeysApiService;

        public JourneysController(IJourneysApiService journeysApiService)
        {
            _journeysApiService = journeysApiService;
        }

        [HttpPost("getjourneys")]
        public async Task<IActionResult> GetJourneys([FromBody] JourneyRequest journeyRequest)
        {
            if (journeyRequest == null) // Ensure request is not null
                journeyRequest = new JourneyRequest();

            try
            {
                var journeys = await _journeysApiService.GetJourneysAsync(journeyRequest);

                if (journeys != null)
                {
                    return Ok(new { status = "Success", data = journeys });
                }
                else
                {
                    return BadRequest(new { status = "Error", message = "Failed to retrieve journeys." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = $"Internal server error: {ex.Message}" });
            }
        }
    }

}
