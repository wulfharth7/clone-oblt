using clone_oblt.Services.Interfaces;
using clone_oblt.Utils;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace clone_oblt.Services
{
    public class SessionApiService : Interfaces.ISessionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _sessionApiUrl;
        private readonly string _apiKey;

        public SessionApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _sessionApiUrl = configuration["ApiSettings:SessionApiUrl"]; // OBilet Session Creation Api is stored here.
            _apiKey = SingletonApiKey.GetInstance().ApiKey;
        }

        public async Task<T> PostAsync<T>(object body)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _sessionApiUrl)
            {
                Content = jsonContent
            };

            AddHeadersAndUrlToRequest(requestMessage);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode(); 
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                //will get back to normal once 429 stops xd
                return JsonSerializer.Deserialize<T>("");
                /*Console.WriteLine($"Request failed: {ex.Message}");
                throw new Exception($"Request failed with status code: {ex.Message}");*/
            }
        }

        private void AddHeadersAndUrlToRequest(HttpRequestMessage requestMessage)
        {
            var headers = new
            {
                Authorization = $"Basic {_apiKey}",
                ContentType = "application/json"
            };

            var headerDictionary = ConvertHeadersToDictionary(headers);

            foreach (var header in headerDictionary)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
                Console.WriteLine($"Header: {header.Key} = {header.Value}");
            }
        }
        private Dictionary<string, string> ConvertHeadersToDictionary(object headers)
        {
            var headerDictionary = new Dictionary<string, string>();
            var properties = headers.GetType().GetProperties();
            foreach (var property in properties)
            {
                headerDictionary.Add(property.Name, property.GetValue(headers)?.ToString());
            }
            return headerDictionary;
        }
    }
}
