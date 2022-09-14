using Microsoft.Azure.Devices;

namespace Shared.Models.Response.Devices;

public class HttpDeviceResponse
{
    public HttpDeviceResponse()
    {

    }
    public HttpDeviceResponse(string message)
    {
        Message = message;
    }

    public HttpDeviceResponse(string message, string exception)
    {
        Message = message;
        Exception = exception;
    }

    public HttpDeviceResponse(string message, Device device)
    {
        Message = message;

        if (device != null)
            ConnectionString = $"HostName={Environment.GetEnvironmentVariable("IotHub")?.Split(';')[0].Split('=')[1]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";

    }

    public string Message { get; set; } = null!;
    public string Exception { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
}