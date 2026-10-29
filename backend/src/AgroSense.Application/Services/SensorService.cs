using Microsoft.EntityFrameworkCore;
using AgroSense.Application.Common.Interfaces;
using AgroSense.Application.DTOs;
using AgroSense.Application.Interfaces;
using AgroSense.Domain.Entities;

namespace AgroSense.Application.Services;

public class SensorService : ISensorService
{
    private readonly IAppDbContext _db;
    private readonly IMLService _mlService;

    public SensorService(IAppDbContext db, IMLService mlService)
    {
        _db = db;
        _mlService = mlService;
    }

    public async Task<(SensorReading Reading, MLPredictionResultDto? Prediction)> IngestReadingAsync(SensorReadingDto dto, CancellationToken cancellationToken = default)
    {
        var reading = new SensorReading
        {
            SoilMoisture = dto.SoilMoisture,
            Temperature = dto.Temperature,
            Humidity = dto.Humidity,
            Source = dto.Source,
            DeviceId = dto.DeviceId,
            Timestamp = DateTime.UtcNow
        };

        _db.SensorReadings.Add(reading);
        await _db.SaveChangesAsync(cancellationToken);

        // Auto-predict via ML service
        var predictionResult = await _mlService.PredictAsync(dto.SoilMoisture, dto.Temperature, dto.Humidity, "iot", cancellationToken);

        if (predictionResult != null)
        {
            var prediction = new Prediction
            {
                SoilMoisture = dto.SoilMoisture,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity,
                WaterStressRisk = predictionResult.water_stress_risk,
                IrrigationRecommendation = predictionResult.irrigation_recommendation,
                Confidence = predictionResult.confidence,
                Explanation = predictionResult.explanation,
                Source = "iot",
                Timestamp = DateTime.UtcNow
            };
            _db.Predictions.Add(prediction);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return (reading, predictionResult);
    }

    public async Task<IEnumerable<SensorReading>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        return await _db.SensorReadings
            .OrderByDescending(r => r.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<SensorReading?> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        return await _db.SensorReadings
            .OrderByDescending(r => r.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

