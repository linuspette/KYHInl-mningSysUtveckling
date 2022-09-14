using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataEntities;

public class RoleEntity
{
    [Required, Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}