using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Models.Input.Devices;

public class AddDeviceRequest
{
    [Required]
    public string DeviceId { get; set; } = null!;
    public string DeviceType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Owner { get; set; } = null!;
}