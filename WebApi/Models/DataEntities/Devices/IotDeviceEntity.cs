using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.DataEntities.Devices;

[Index(nameof(DeviceId), IsUnique = true)]
public class IotDeviceEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required, Column(TypeName = "varchar(150)")]
    public string DeviceId { get; set; } = null!;
    [Required, Column(TypeName = "varchar(150)")]
    public string Owner { get; set; } = null!;
    [Required, Column(TypeName = "varchar(150)")]
    public string Location { get; set; } = null!;
    [Required, Column(TypeName = "varchar(150)")]
    public string DeviceType { get; set; } = null!;
    [Required] 
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;    
    [Required] 
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}