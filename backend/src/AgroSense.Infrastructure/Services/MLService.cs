using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using AgroSense.Application.Common.Interfaces;
using AgroSense.Application.DTOs;

namespace AgroSense.Infrastructure.Services;

public class MLService : IMLService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public MLService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<MLPredictionResultDto?> PredictAsync(float soilMoisture, float temperature, float humidity, string source, CancellationToken cancellationToken = default)
    {
        var mlServiceUrl = _config["MLService:BaseUrl"] ?? "http://localhost:8000";

        var payload = new
        {
            soil_moisture = soilMoisture,
            temperature = temperature,
            humidity = humidity,
            source = source
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{mlServiceUrl}/predict", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<MLPredictionResultDto>(cancellationToken: cancellationToken);
        }
        catch
        {
            return null;
        }
    }
}

