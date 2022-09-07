using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace ClassLibrary.Models.Response.Devices;

public class AddDeviceResponse
{
    public string Message { get; set; } = null!;
    public string IotHub { get; set; } = null!;
    public Device Device { get; set; } = null!;
    public Twin DeviceTwin { get; set; } = null!;
}