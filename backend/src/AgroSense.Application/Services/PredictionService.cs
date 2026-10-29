using Microsoft.EntityFrameworkCore;
using AgroSense.Application.Common.Interfaces;
using AgroSense.Application.DTOs;
using AgroSense.Application.Interfaces;
using AgroSense.Domain.Entities;

namespace AgroSense.Application.Services;

public class PredictionService : IPredictionService
{
    private readonly IAppDbContext _db;
    private readonly IMLService _mlService;

    public PredictionService(IAppDbContext db, IMLService mlService)
    {
        _db = db;
        _mlService = mlService;
    }

    public async Task<PredictionResponseDto?> PredictAsync(PredictRequestDto dto, CancellationToken cancellationToken = default)
    {
        var mlResult = await _mlService.PredictAsync(dto.SoilMoisture, dto.Temperature, dto.Humidity, dto.Source, cancellationToken);
        if (mlResult == null)
            return null;

        var prediction = new Prediction
        {
            SoilMoisture = dto.SoilMoisture,
            Temperature = dto.Temperature,
            Humidity = dto.Humidity,
            WaterStressRisk = mlResult.water_stress_risk,
            IrrigationRecommendation = mlResult.irrigation_recommendation,
            Confidence = mlResult.confidence,
            Explanation = mlResult.explanation,
            Source = dto.Source,
            Timestamp = DateTime.UtcNow
        };

        _db.Predictions.Add(prediction);
        await _db.SaveChangesAsync(cancellationToken);

        return new PredictionResponseDto
        {
            WaterStressRisk = prediction.WaterStressRisk,
            IrrigationRecommendation = prediction.IrrigationRecommendation,
            Confidence = prediction.Confidence,
            Explanation = prediction.Explanation,
            Timestamp = prediction.Timestamp
        };
    }

    public async Task<IEnumerable<Prediction>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        return await _db.Predictions
            .OrderByDescending(p => p.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}

