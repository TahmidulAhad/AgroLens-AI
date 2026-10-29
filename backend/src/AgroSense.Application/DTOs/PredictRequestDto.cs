namespace AgroSense.Application.DTOs;

public class PredictRequestDto
{
    public float SoilMoisture { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public string Source { get; set; } = "manual"; // "iot" or "manual"
}

