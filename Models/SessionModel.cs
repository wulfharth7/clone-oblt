using Newtonsoft.Json;

namespace clone_oblt.Models
{
    public class SessionRequest
    {
        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("connection")]
        public ConnectionInfo? Connection { get; set; }

        [JsonProperty("browser")]
        public BrowserInfo? Browser { get; set; }
    }

    public class ConnectionInfo
    {
        [JsonProperty("ip-address")]
        public string? IpAddress { get; set; }

        [JsonProperty("port")]
        public string? Port { get; set; }
    }

    public class BrowserInfo
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }
    }

    public class SessionResponse
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("data")]
        public SessionData? Data { get; set; }
    }

    public class SessionData
    {
        [JsonProperty("session-id")]
        public string? SessionId { get; set; }

        [JsonProperty("device-id")]
        public string? DeviceId { get; set; }

        public string? Affiliate { get; set; }

        public int? DeviceType { get; set; }

        public string? Device { get; set; }

        [JsonProperty("ip-country")]
        public string? IpCountry { get; set; }
    }
}
