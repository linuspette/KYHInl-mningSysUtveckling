namespace LpSmartHub.MVVM.Models;

public class DirectMethodRequest
{
    public string DeviceId { get; set; } = null!;
    public string MethodName { get; set; } = null!;
    public object? Payload { get; set; }
}