using Microsoft.EntityFrameworkCore;
using WebApi.Models.DataEntities;

namespace WebApi.Helpers;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<RoleEntity> Roles { get; set; } = null!;
    public DbSet<JwtTokenEntity> Tokens { get; set; } = null!;
}