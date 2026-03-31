using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Oasis.Domain;

namespace Oasis.Infrastructure.Persistence;

public class OasisDbContext(DbContextOptions<OasisDbContext> options) : IdentityDbContext<User>(options)
{

    /// <summary>
    /// Cabins DbSet
    /// </summary>
    public DbSet<Cabin> Cabins { get; set; }

    /// <summary>
    /// Bookings DbSet
    /// </summary>
    public DbSet<Booking> Bookings { get; set; }


    /// <summary>
    /// Guests DbSet
    /// </summary>
    public DbSet<Guest> Guests { get; set; }

    /// <summary>
    /// RefreshTokens DbSet
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }


    /// <summary>
    /// Configure the model
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OasisDbContext).Assembly);
    }

    /// <summary>
    /// Configure the DbContext options
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) //设置查询跟踪行为 默认不跟踪
                      .EnableSensitiveDataLogging(); // 启用敏感数据日志记录
    }

}