using AgroSense.Application.DTOs;
using AgroSense.Domain.Entities;

namespace AgroSense.Application.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<Device>> GetActiveDevicesAsync(CancellationToken cancellationToken = default);
    Task<Device> RegisterDeviceAsync(DeviceDto dto, CancellationToken cancellationToken = default);
}

