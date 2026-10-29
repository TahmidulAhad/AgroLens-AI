namespace AgroSense.Domain.Entities;

public class SensorReading
{
    public int Id { get; set; }
    public float SoilMoisture { get; set; }     // percentage 0-100
    public float Temperature { get; set; }       // Celsius
    public float Humidity { get; set; }          // percentage 0-100
    public string Source { get; set; } = "iot"; // "iot" or "manual"
    public string DeviceId { get; set; } = "unknown";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

