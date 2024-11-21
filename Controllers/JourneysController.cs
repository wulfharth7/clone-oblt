using clone_oblt.Helpers;
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
            try
            {
                var journeys = await _journeysApiService.GetJourneysAsync(journeyRequest); //This function basically returns us the bus ticket vendors' tickets
                return ResponseUtil.Success(journeys);                                     //So people can see at what time there is a ticket and its details.
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message.ToString());
            }
        }
    }

}
