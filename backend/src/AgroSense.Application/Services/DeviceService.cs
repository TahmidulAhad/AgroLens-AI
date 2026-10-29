using Microsoft.EntityFrameworkCore;
using AgroSense.Application.Common.Interfaces;
using AgroSense.Application.DTOs;
using AgroSense.Application.Interfaces;
using AgroSense.Domain.Entities;

namespace AgroSense.Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IAppDbContext _db;

    public DeviceService(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Device>> GetActiveDevicesAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Devices
            .Where(d => d.IsActive)
            .OrderByDescending(d => d.LastSeen)
            .ToListAsync(cancellationToken);
    }

    public async Task<Device> RegisterDeviceAsync(DeviceDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Devices
            .FirstOrDefaultAsync(d => d.DeviceId == dto.DeviceId, cancellationToken);

        if (existing != null)
        {
            existing.Name = dto.Name;
            existing.Location = dto.Location;
            existing.LastSeen = DateTime.UtcNow;
            existing.IsActive = true;
            await _db.SaveChangesAsync(cancellationToken);
            return existing;
        }

        var device = new Device
        {
            DeviceId = dto.DeviceId,
            Name = dto.Name,
            Location = dto.Location,
            IsActive = true,
            RegisteredAt = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow
        };

        _db.Devices.Add(device);
        await _db.SaveChangesAsync(cancellationToken);
        return device;
    }
}

