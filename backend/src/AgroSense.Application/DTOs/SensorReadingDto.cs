namespace AgroSense.Application.DTOs;

public class SensorReadingDto
{
    public float SoilMoisture { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public string Source { get; set; } = "iot";
    public string DeviceId { get; set; } = "unknown";
}

