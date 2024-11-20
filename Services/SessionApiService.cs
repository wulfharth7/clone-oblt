using clone_oblt.Models;
using clone_oblt.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace clone_oblt.Services
{
    public class SessionApiService : ApiServiceBase, Interfaces.ISessionApiService
    {
        public SessionApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration, "ApiSettings:SessionApiUrl")
        {
        }

        public async Task<T> PostAsync<T>(object body)
        {
            return await SendRequestAsync<object, T>(body, _apiUrl);
        }
    }
}
