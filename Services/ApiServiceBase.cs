using clone_oblt.Utils;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using clone_oblt.Services.Interfaces;

namespace clone_oblt.Services
{
    public abstract class ApiServiceBase : IApiServiceBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _apiUrl;
        protected readonly string _apiKey;

        public ApiServiceBase(HttpClient httpClient, IConfiguration configuration, string apiUrlKey)
        {
            _httpClient = httpClient;
            _apiUrl = configuration[apiUrlKey];
            _apiKey = SingletonApiKey.GetInstance().ApiKey;
        }

        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest requestBody, string endpoint)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint) { Content = jsonContent };

            HeaderUtils.AddHeadersToRequest(requestMessage, _apiKey);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Request failed: {ex.Message}");
            }
        }
    }
}
