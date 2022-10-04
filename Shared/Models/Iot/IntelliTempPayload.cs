namespace Shared.Models.Iot;

public class IntelliTempPayload
{
    public string DeviceId { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Type { get; set; } = null!;
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public DateTime LocalTimestamp { get; set; } = DateTime.Now.ToLocalTime();
}