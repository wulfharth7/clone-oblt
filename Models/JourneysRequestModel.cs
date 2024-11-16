using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace clone_oblt.Models
{
    //todo api-request-id etc.
    //check docs again, skipped some response fields.
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
        public int? OriginId { get; set; }

        [JsonProperty("destination-id")]
        public int? DestinationId { get; set; }

        [JsonProperty("departure-date")]
        public DateTime? DepartureDate { get; set; }
    }

    public class JourneyResponse
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("data")]
        public List<JourneyDetails>? Data { get; set; }
    }

    // Details of each journey in the response
    public class JourneyDetails
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("partner-id")]
        public int? PartnerId { get; set; }

        [JsonProperty("partner-name")]
        public string? PartnerName { get; set; }

        [JsonProperty("route-id")]
        public int? RouteId { get; set; }

        [JsonProperty("bus-type")]
        public string? BusType { get; set; }

        [JsonProperty("total-seats")]
        public int? TotalSeats { get; set; }

        [JsonProperty("available-seats")]
        public int? AvailableSeats { get; set; }

        [JsonProperty("journey")]
        public JourneyInfo? Journey { get; set; }

        [JsonProperty("features")]
        public List<JourneyFeature>? Features { get; set; }

        [JsonProperty("origin-location")]
        public string? OriginLocation { get; set; }

        [JsonProperty("destination-location")]
        public string? DestinationLocation { get; set; }

        [JsonProperty("is-active")]
        public bool? IsActive { get; set; }

        [JsonProperty("origin-location-id")]
        public int? OriginLocationId { get; set; }

        [JsonProperty("destination-location-id")]
        public int? DestinationLocationId { get; set; }

        [JsonProperty("partner-rating")]
        public double? PartnerRating { get; set; }
    }

    public class JourneyInfo
    {
        [JsonProperty("kind")]
        public string? Kind { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("stops")]
        public List<JourneyStop>? Stops { get; set; }

        [JsonProperty("origin")]
        public string? Origin { get; set; }

        [JsonProperty("destination")]
        public string? Destination { get; set; }

        [JsonProperty("departure")]
        public DateTime? Departure { get; set; }

        [JsonProperty("arrival")]
        public DateTime? Arrival { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("duration")]
        public string? Duration { get; set; }

        [JsonProperty("original-price")]
        public double? OriginalPrice { get; set; }

        [JsonProperty("internet-price")]
        public double? InternetPrice { get; set; }

        [JsonProperty("bus-name")]
        public string? BusName { get; set; }

        [JsonProperty("policy")]
        public JourneyPolicy? Policy { get; set; }
    }

    public class JourneyStop
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("station")]
        public string? Station { get; set; }

        [JsonProperty("time")]
        public DateTime? Time { get; set; }

        [JsonProperty("is-origin")]
        public bool? IsOrigin { get; set; }

        [JsonProperty("is-destination")]
        public bool? IsDestination { get; set; }
    }

    public class JourneyPolicy
    {
        [JsonProperty("max-seats")]
        public int? MaxSeats { get; set; }

        [JsonProperty("max-single")]
        public int? MaxSingle { get; set; }

        [JsonProperty("max-single-males")]
        public int? MaxSingleMales { get; set; }

        [JsonProperty("max-single-females")]
        public int? MaxSingleFemales { get; set; }

        [JsonProperty("mixed-genders")]
        public bool? MixedGenders { get; set; }

        [JsonProperty("gov-id")]
        public bool? GovId { get; set; }

        [JsonProperty("lht")]
        public bool? Lht { get; set; }
    }

    public class JourneyFeature
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("priority")]
        public int? Priority { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("is-promoted")]
        public bool? IsPromoted { get; set; }
    }
}
