using clone_oblt.Models;
using System.Threading.Tasks;

namespace clone_oblt.Services.Interfaces
{
    public interface IBusLocationApiService
    {
        Task<List<BusLocationData>> GetBusLocationsAsync(BusLocationRequest requestbody);
    }
}
