using clone_oblt.Utils;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using clone_oblt.Services.Interfaces;

namespace clone_oblt.Services
{
    //Api Base Service actually helps us to make the code more readable and modular.
    //In every api service class, I had a need to set the headers and change the payload.
    //Some values in the header or payload are not dynamic and they dont change.
    //But for future purposes, if they would change etc. this class was a good approach to solve that problem

    public abstract class ApiServiceBase : IApiServiceBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _apiUrl;
        protected readonly string _apiKey;
        public ApiServiceBase(HttpClient httpClient, IConfiguration configuration, string apiUrlKey)
        {
            _httpClient = httpClient;
            _apiUrl = configuration[apiUrlKey];
            _apiKey = SingletonApiKeyUtil.GetInstance().ApiKey;
        }

        //Because all of the api endpoints that we are using right now are POST requests, I didn't implement any other type HttpMethod yet.
        //But as you can see from the class, its really to do so!
        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest requestBody, string endpoint)
        {
            var serializedRequestBody = JsonConvert.SerializeObject(requestBody);
            var jsonContent = new StringContent(serializedRequestBody, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = jsonContent
            };

            HeaderUtil.AddHeadersToRequest(requestMessage, _apiKey);

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
