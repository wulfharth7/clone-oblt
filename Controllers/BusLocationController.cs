using clone_oblt.Helpers;
using clone_oblt.Models;
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
        public async Task<IActionResult> GetBusLocations([FromBody] BusLocationRequest requestbody)
        {
            try
            {
                var busLocations = await _busLocationApiService.GetBusLocationsAsync(requestbody);
                return ResponseUtil.Success(busLocations);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message.ToString());
            }
        }
    }
}
