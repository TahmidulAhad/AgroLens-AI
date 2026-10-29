using Microsoft.EntityFrameworkCore;
using AgroSense.Domain.Entities;

namespace AgroSense.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<SensorReading> SensorReadings { get; }
    DbSet<Prediction> Predictions { get; }
    DbSet<Device> Devices { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

