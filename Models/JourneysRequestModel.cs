using Newtonsoft.Json;
using System;

namespace clone_oblt.Models
{
    public class JourneyRequest
    {
        [JsonProperty("device-session")]
        public DeviceSession? DeviceSession { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("language")]
        public string? Language { get; set; }

        [JsonProperty("data")]
        public JourneyData? Data { get; set; } 
    }


    public class JourneyData
    {
        [JsonProperty("origin-id")]
        public int OriginId { get; set; }

        [JsonProperty("destination-id")]
        public int DestinationId { get; set; }

        [JsonProperty("departure-date")]
        public DateTime DepartureDate { get; set; }
    }

    public class JourneyResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public JourneyData Data { get; set; }
    }
}