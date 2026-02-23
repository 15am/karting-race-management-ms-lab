using Karting.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence;

public sealed class KartingDbContext(DbContextOptions<KartingDbContext> options) : DbContext(options)
{
    public DbSet<DriverEntity> Drivers => Set<DriverEntity>();

    public DbSet<RaceEntity> Races => Set<RaceEntity>();

    public DbSet<LapTimeEntity> LapTimes => Set<LapTimeEntity>();

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
