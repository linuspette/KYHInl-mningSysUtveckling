using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace ClassLibrary.Models.Response.Devices;

public class AddDeviceResponse
{
    public string? Message { get; set; }
    public string? IotHubName { get; set; }
    public string DeviceConnectionString => $"Hostname={IotHubName};DeviceId={Device.Id};SharedAccessKey={Device.Authentication.SymmetricKey.PrimaryKey}";
    public Device Device { get; set; } = null!;
    public Twin DeviceTwin { get; set; } = null!;
}