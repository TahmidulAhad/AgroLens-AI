using AgroSense.Application.DTOs;

namespace AgroSense.Application.Common.Interfaces;

public interface IMLService
{
    Task<MLPredictionResultDto?> PredictAsync(float soilMoisture, float temperature, float humidity, string source, CancellationToken cancellationToken = default);
}

