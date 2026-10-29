namespace AgroSense.Application.DTOs;

public class PredictionResponseDto
{
    public string WaterStressRisk { get; set; } = "";
    public string IrrigationRecommendation { get; set; } = "";
    public float Confidence { get; set; }
    public string Explanation { get; set; } = "";
    public DateTime Timestamp { get; set; }
}

