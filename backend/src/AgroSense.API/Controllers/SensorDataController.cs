using Microsoft.AspNetCore.Mvc;
using AgroSense.Application.DTOs;
using AgroSense.Application.Interfaces;

namespace AgroSense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorDataController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public SensorDataController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    // POST /api/sensordata — Ingest IoT reading from ESP32
    [HttpPost]
    public async Task<IActionResult> IngestSensorData([FromBody] SensorReadingDto dto, CancellationToken cancellationToken)
    {
        var (reading, prediction) = await _sensorService.IngestReadingAsync(dto, cancellationToken);
        return Ok(new { reading, prediction });
    }

    // GET /api/sensordata?page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var history = await _sensorService.GetHistoryAsync(page, pageSize, cancellationToken);
        return Ok(history);
    }

    // GET /api/sensordata/latest
    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest(CancellationToken cancellationToken)
    {
        var latest = await _sensorService.GetLatestAsync(cancellationToken);
        return Ok(latest);
    }
}

