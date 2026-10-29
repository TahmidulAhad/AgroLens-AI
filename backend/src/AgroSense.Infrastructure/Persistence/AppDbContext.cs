using Microsoft.EntityFrameworkCore;
using AgroSense.Application.Common.Interfaces;
using AgroSense.Domain.Entities;

namespace AgroSense.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
    public DbSet<Prediction> Predictions => Set<Prediction>();
    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SensorReading>(entity =>
        {
            entity.ToTable("sensor_readings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SoilMoisture).IsRequired();
            entity.Property(e => e.Temperature).IsRequired();
            entity.Property(e => e.Humidity).IsRequired();
            entity.Property(e => e.Source).HasMaxLength(20).HasDefaultValue("iot");
            entity.Property(e => e.DeviceId).HasMaxLength(100).HasDefaultValue("unknown");
            entity.Property(e => e.Timestamp).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Prediction>(entity =>
        {
            entity.ToTable("predictions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WaterStressRisk).HasMaxLength(20);
            entity.Property(e => e.IrrigationRecommendation).HasMaxLength(50);
            entity.Property(e => e.Source).HasMaxLength(20).HasDefaultValue("manual");
            entity.Property(e => e.Timestamp).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("devices");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DeviceId).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.DeviceId).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RegisteredAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.LastSeen).HasDefaultValueSql("NOW()");
        });
    }
}

