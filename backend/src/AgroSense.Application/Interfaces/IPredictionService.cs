using AgroSense.Application.DTOs;
using AgroSense.Domain.Entities;

namespace AgroSense.Application.Interfaces;

public interface IPredictionService
{
    Task<PredictionResponseDto?> PredictAsync(PredictRequestDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<Prediction>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
}

