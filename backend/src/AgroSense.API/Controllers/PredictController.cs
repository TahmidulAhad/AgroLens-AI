using Microsoft.AspNetCore.Mvc;
using AgroSense.Application.DTOs;
using AgroSense.Application.Interfaces;

namespace AgroSense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PredictController : ControllerBase
{
    private readonly IPredictionService _predictionService;

    public PredictController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    // POST /api/predict — Manual or IoT input inference
    [HttpPost]
    public async Task<IActionResult> Predict([FromBody] PredictRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await _predictionService.PredictAsync(dto, cancellationToken);
        if (result == null)
            return StatusCode(503, new { error = "ML Inference service is currently unreachable or starting up." });

        return Ok(result);
    }

    // GET /api/predict?page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetPredictions([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var predictions = await _predictionService.GetHistoryAsync(page, pageSize, cancellationToken);
        return Ok(predictions);
    }
}

