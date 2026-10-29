using Microsoft.AspNetCore.Mvc;
using AgroSense.Application.DTOs;
using AgroSense.Application.Interfaces;

namespace AgroSense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices(CancellationToken cancellationToken)
    {
        var devices = await _deviceService.GetActiveDevicesAsync(cancellationToken);
        return Ok(devices);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterDevice([FromBody] DeviceDto dto, CancellationToken cancellationToken)
    {
        var device = await _deviceService.RegisterDeviceAsync(dto, cancellationToken);
        return Ok(device);
    }
}

