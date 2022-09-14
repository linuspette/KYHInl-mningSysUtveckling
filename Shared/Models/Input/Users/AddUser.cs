using Shared.Models.View.Roles;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Input.Users;

public class AddUser
{
    [Required, RegularExpression(@"((?=.*\d)(?=.*[A-Z]).{8,})")]
    public string Username { get; set; } = null!;
    [Required, RegularExpression(@"((?=.*\d)(?=.*[A-Z]).{8,})")]
    public string Password { get; set; } = null!;

    public List<Role>? Type { get; set; }
}