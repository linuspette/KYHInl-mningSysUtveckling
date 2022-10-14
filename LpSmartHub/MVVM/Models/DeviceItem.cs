namespace LpSmartHub.MVVM.Models;

public class DeviceItem
{
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Interval { get; set; }
    public bool DeviceState { get; set; }
    public string IconActive { get; set; } = string.Empty;
    public string IconInActive { get; set; } = string.Empty;
}