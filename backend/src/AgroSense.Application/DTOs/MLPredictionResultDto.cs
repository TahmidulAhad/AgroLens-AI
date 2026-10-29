namespace AgroSense.Application.DTOs;

public class MLPredictionResultDto
{
    public string water_stress_risk { get; set; } = "";
    public string irrigation_recommendation { get; set; } = "";
    public float confidence { get; set; }
    public string explanation { get; set; } = "";
}

