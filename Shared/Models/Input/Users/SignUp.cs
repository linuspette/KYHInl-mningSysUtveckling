using Shared.Models.View.Roles;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Input.Users;

public class SignUp
{
    [Required, RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{3,20}$")]
    public string Username { get; set; } = null!;
    [Required, RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,})")]
    public string Password { get; set; } = null!;
}