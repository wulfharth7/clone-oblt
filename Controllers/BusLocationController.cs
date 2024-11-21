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
        public async Task<IActionResult> GetBusLocations([FromBody] BusLocationRequest requestbody) //Basically, the controller for the page, where user
        {                                                                                           //chooses their destination in the page to go.
            try                                                                                     //Before clicking the search button, they use this function.
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
