namespace Shared.Models.Iot;

public class IntelliTempPayload
{
    public string DeviceId { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Temperature { get; set; } = null!;
    public string Humidity { get; set; } = null!;
    public DateTime LocalTimestamp { get; set; } = DateTime.Now.ToLocalTime();
}