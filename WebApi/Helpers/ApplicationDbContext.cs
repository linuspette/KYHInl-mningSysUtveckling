using Microsoft.EntityFrameworkCore;
using WebApi.Models.DataEntities.Authentication;
using WebApi.Models.DataEntities.Devices;

namespace WebApi.Helpers;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<JwtTokenEntity> Tokens { get; set; } = null!;
    public DbSet<IotDeviceEntity> IotDevices { get; set; } = null!;
}