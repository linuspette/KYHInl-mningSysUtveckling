namespace Shared.Models.View.Device;

public class IotDevice
{
    public string DeviceId { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string DeviceType { get; set; } = null!;
}