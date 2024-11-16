using System;
using Newtonsoft.Json;

namespace clone_oblt.Models
{
    public class BusLocationRequest
    {
        [JsonProperty("data")]
        public string? Data { get; set; }

        [JsonProperty("device-session")]
        public DeviceSession? DeviceSession { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("language")]
        public string? Language { get; set; }
    }

    public class DeviceSession
    {
        [JsonProperty("session-id")]
        public string? SessionId { get; set; }

        [JsonProperty("device-id")]
        public string? DeviceId { get; set; }
    }

    public class BusLocationResponse
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("data")]
        public BusLocationData[]? Data { get; set; }
    }

    public class BusLocationData
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("parent-id")]
        public int? ParentId { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("geo-location")]
        public GeoLocation? GeoLocation { get; set; }

        [JsonProperty("tz-code")]
        public string? TzCode { get; set; }

        [JsonProperty("weather-code")]
        public string? WeatherCode { get; set; }

        [JsonProperty("rank")]
        public int? Rank { get; set; }

        [JsonProperty("reference-code")]
        public string? ReferenceCode { get; set; }

        [JsonProperty("keywords")]
        public string? Keywords { get; set; }
    }

    public class GeoLocation
    {
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [JsonProperty("zoom")]
        public int? Zoom { get; set; }
    }
}
