using clone_oblt.Builders;
using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;

namespace clone_oblt.Services
{

    //Api services in this project are our "proxies" to communicate with the api of obilet.
    //For those specific endpoints, we use these classes, send request over them using the builders and session ids.
    public class BusLocationApiService : ApiServiceBase, IBusLocationApiService
    {
        private readonly ISessionHelperService _sessionHelperService;

        public BusLocationApiService(HttpClient httpClient, IConfiguration configuration, ISessionHelperService sessionHelperService)
            : base(httpClient, configuration, "ApiSettings:BusLocationsApiUrl")
        {
            _sessionHelperService = sessionHelperService;
        }

        //This is the function, where the client chooses their destination to go.
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
