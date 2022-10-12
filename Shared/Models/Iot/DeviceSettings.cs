using Newtonsoft.Json;

namespace Shared.Models.Iot;

public class DeviceSettings
{
    [JsonProperty("deviceId")]
    public string DeviceId { get; set; } = null!;
    [JsonProperty("deviceName")]
    public string DeviceName { get; set; } = null!;
    [JsonProperty("owner")]
    public string Owner { get; set; } = null!;
    [JsonProperty("location")]
    public string Location { get; set; } = null!;
    [JsonProperty("deviceType")]
    public string DeviceType { get; set; } = null!;
    [JsonProperty("interval")]
    public int Interval { get; set; }
    [JsonProperty("deviceState")]
    public bool DeviceState { get; set; }
}