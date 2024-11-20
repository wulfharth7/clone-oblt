using clone_oblt.Builders;
using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace clone_oblt.Services
{
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

            var response = await SendRequestAsync<SessionRequest,SessionResponse>(request, _apiUrl);
            return response;
        }
    }
}
