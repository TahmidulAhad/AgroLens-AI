using AgroSense.Application.DTOs;
using AgroSense.Domain.Entities;

namespace AgroSense.Application.Interfaces;

public interface ISensorService
{
    Task<(SensorReading Reading, MLPredictionResultDto? Prediction)> IngestReadingAsync(SensorReadingDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<SensorReading>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<SensorReading?> GetLatestAsync(CancellationToken cancellationToken = default);
}

