using clone_oblt.Builders;
using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;

namespace clone_oblt.Services
{
    public class BusLocationApiService : ApiServiceBase, IBusLocationApiService
    {
        private readonly ISessionHelperService _sessionHelperService;

        public BusLocationApiService(HttpClient httpClient, IConfiguration configuration, ISessionHelperService sessionHelperService)
            : base(httpClient, configuration, "ApiSettings:BusLocationsApiUrl")
        {
            _sessionHelperService = sessionHelperService;
        }

        public async Task<List<BusLocationData>> GetBusLocationsAsync(BusLocationRequest requestbody)
        {
            var (sessionId, deviceId) = _sessionHelperService.GetSessionInfo();
            var request = new BusLocationBuilder()
                .WithBusLocationData(requestbody.Data)
                .WithDeviceSession(sessionId, deviceId)
                .WithDate(DateTime.Now)
                .WithLanguage("tr-TR")
                .Build();

            var busLocationResponse = await SendRequestAsync<BusLocationRequest, BusLocationResponse>(request, _apiUrl);
            if (busLocationResponse == null)
                throw new Exception("Bus Location could not be found.");

            return busLocationResponse?.Data?.ToList();
        }
    }
}
