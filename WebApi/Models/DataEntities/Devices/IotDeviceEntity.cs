namespace WebApi.Models.DataEntities.Devices;

public class IotDeviceEntity
{
    public Guid Id { get; set; }
    public string DeviceId { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string DeviceType { get; set; } = null!;

}