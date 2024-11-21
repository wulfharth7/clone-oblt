using System.Net.Http;

namespace clone_oblt.Utils
{
    public static class HeaderUtil
    {
        public static void AddHeadersToRequest(HttpRequestMessage requestMessage, string _apiKey)
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

        private static Dictionary<string, string> ConvertHeadersToDictionary(object headers)
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
