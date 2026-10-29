namespace AgroSense.Domain.Entities;

public class Prediction
{
    public int Id { get; set; }
    public float SoilMoisture { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public string WaterStressRisk { get; set; } = "";   // Low / Medium / High
    public string IrrigationRecommendation { get; set; } = ""; // Needed / Monitor / No Action
    public float Confidence { get; set; }
    public string Explanation { get; set; } = "";
    public string Source { get; set; } = "manual";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

