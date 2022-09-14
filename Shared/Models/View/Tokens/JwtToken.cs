using System.ComponentModel.DataAnnotations;

namespace Shared.Models.View.Tokens;

public class JwtToken
{
    [Key]
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
}