using System.Text.Json.Serialization;

namespace clone_oblt.Models
{
    public class CreateSessionRequest
    {
        public int Type { get; set; }

        public ConnectionInfo Connection { get; set; }

        public BrowserInfo Browser { get; set; }
    }

    public class ConnectionInfo
    {
        [JsonPropertyName("ip-address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("port")]
        public string Port { get; set; }
    }

    public class BrowserInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }

    public class CreateSessionResponse
    {
        [JsonPropertyName("status")] 
        public string Status { get; set; }
        [JsonPropertyName("data")]
        public SessionData Data { get; set; }
    }

    public class SessionData
    {
        [JsonPropertyName("session-id")]
        public string SessionId { get; set; }

        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }

        public string Affiliate { get; set; }

        public int DeviceType { get; set; }

        public string Device { get; set; }

        [JsonPropertyName("ip-country")]
        public string IpCountry { get; set; }
    }
}
