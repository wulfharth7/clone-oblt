using clone_oblt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace clone_oblt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusLocationController : Controller
    {
        private readonly IBusLocationApiService _busLocationApiService;

        public BusLocationController(IBusLocationApiService busLocationApiService)
        {
            _busLocationApiService = busLocationApiService;
        }

        [HttpPost("getbuslocations")]
        public async Task<IActionResult> GetBusLocations()
        {
            try
            {
                var busLocations = await _busLocationApiService.GetBusLocationsAsync();

                if (busLocations != null)
                {
                    return Ok(new
                    {
                        status = "Success",
                        data = busLocations
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        status = "Error",
                        message = "Failed to retrieve bus locations."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Error",
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }
    }
}
