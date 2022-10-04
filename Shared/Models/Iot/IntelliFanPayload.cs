namespace Shared.Models.Iot;

public class IntelliFanPayload
{
    public string DeviceId { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsRunning { get; set; }
    public DateTime LocalTimestamp { get; set; } = DateTime.Now.ToLocalTime();
}