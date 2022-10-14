using Newtonsoft.Json;

namespace Shared.Models.Input.Devices;

public class HttpDeviceRequest
{
    [JsonProperty("deviceId")]
    public string DeviceId { get; set; } = null!;
}