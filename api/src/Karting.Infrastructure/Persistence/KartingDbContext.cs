using Karting.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence;

/// <summary>
/// Represents the KartingDbContext class.
/// </summary>
public sealed class KartingDbContext(DbContextOptions<KartingDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Represents a publicly exposed member.
    /// </summary>
    public DbSet<DriverEntity> Drivers => Set<DriverEntity>();

    /// <summary>
    /// Represents a publicly exposed member.
    /// </summary>
    public DbSet<RaceEntity> Races => Set<RaceEntity>();

    /// <summary>
    /// Represents a publicly exposed member.
    /// </summary>
    public DbSet<LapTimeEntity> LapTimes => Set<LapTimeEntity>();

    /// <summary>
    /// Executes OnModelCreating.
    /// </summary>
    /// <param name="modelBuilder">The modelBuilder value.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DriverEntity>(entity =>
        {
            entity.ToTable("Drivers");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(80).IsRequired();
            entity.Property(x => x.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<RaceEntity>(entity =>
        {
            entity.ToTable("Races");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.DateUtc).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(40).IsRequired();
        });

        modelBuilder.Entity<LapTimeEntity>(entity =>
        {
            entity.ToTable("LapTimes");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.LapNumber).IsRequired();
            entity.Property(x => x.TimeMilliseconds).IsRequired();
            entity.Property(x => x.CreatedAtUtc).IsRequired();

            entity.HasOne(x => x.Race)
                .WithMany(x => x.LapTimes)
                .HasForeignKey(x => x.RaceId);

            entity.HasOne(x => x.Driver)
                .WithMany(x => x.LapTimes)
                .HasForeignKey(x => x.DriverId);
        });
    }
}
