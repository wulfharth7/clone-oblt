using clone_oblt.Models;
using System.Threading.Tasks;

namespace clone_oblt.Services.Interfaces
{
    public interface IJourneysApiService
    {
        Task<JourneyData> GetJourneysAsync(JourneyRequest journeyRequest);
    }
}
