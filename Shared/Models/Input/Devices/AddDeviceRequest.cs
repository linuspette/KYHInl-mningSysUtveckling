using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Input.Devices;

public class AddDeviceRequest
{
    [Required]
    public Guid userId { get; set; }
    [Required]
    public string DeviceId { get; set; } = null!;
    public string DeviceType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Owner { get; set; } = null!;
}