using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataEntities.Authentication;

public class JwtTokenEntity
{
    [Required, Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Token { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; } = true;
    [Required]
    public DateTime Created { get; private set; }
    [Required]
    public DateTime Expires { get; private set; }

    public void SetCreationAndExpiration(DateTime date)
    {
        Created = date;
        Expires = date.AddHours(20);
    }
}