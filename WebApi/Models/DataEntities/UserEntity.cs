using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Models.DataEntities;

[Index(nameof(Username), IsUnique = true)]
public class UserEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, Column(TypeName = "varchar(150)")]
    public string Username { get; set; } = null!;

    [Required, Column(TypeName = "varbinary(max)")]
    public byte[] Hash { get; private set; } = null!;

    [Required, Column(TypeName = "varbinary(max)")]
    public byte[] Salt { get; private set; } = null!;

    [Required]
    public bool IsActive { get; set; } = true;
    [Required]
    public List<RoleEntity> Roles { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime LastTimePasswordChanged { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Create new Password for User Account
    /// </summary>
    /// <param name="password">A unicode password</param>
    public void CreatePassword(string password)
    {
        using var hmac = new HMACSHA512();
        Salt = hmac.Key;
        Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }



    public bool ValidatePassword(string password)
    {
        using var hmac = new HMACSHA512(Salt);
        var _hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < Hash.Length; i++)
            if (_hash[i] != Hash[i])
                return false;

        return true;
    }
}