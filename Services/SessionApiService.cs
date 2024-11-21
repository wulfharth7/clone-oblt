using clone_oblt.Builders;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace clone_oblt.Services
{
    //Api services in this project are our "proxies" to communicate with the api of obilet.
    //For those specific endpoints, we use these classes, send request over them using the builders and session ids.
    public class SessionApiService : ApiServiceBase, Interfaces.ISessionApiService
    {
        public SessionApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration, "ApiSettings:SessionApiUrl")
        {
        }
        public async Task<SessionResponse> CreateSessionAsync(SessionRequest requestbody)
        {
            var request = new SessionRequestBuilder()
                    .WithType(1)
                    .WithConnection(requestbody.Connection.IpAddress, "5117")
                    .WithBrowser(requestbody.Browser.Name, requestbody.Browser.Version)
                    .Build();

            var response = await SendRequestAsync<SessionRequest,SessionResponse>(request, _apiUrl); //Here we send a request to obilet and gather our session ids.
            return response;
        }
    }
}
