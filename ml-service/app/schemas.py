from pydantic import BaseModel, Field
from typing import Optional

class PredictRequest(BaseModel):
    soil_moisture: float = Field(..., ge=0, le=100, description="Soil moisture percentage (0-100)")
    temperature: float = Field(..., ge=-10, le=60, description="Temperature in Celsius")
    humidity: float = Field(..., ge=0, le=100, description="Humidity percentage (0-100)")
    source: Optional[str] = Field("manual", description="Input source: 'iot' or 'manual'")

class PredictResponse(BaseModel):
    water_stress_risk: str
    irrigation_recommendation: str
    confidence: float
    explanation: str
    source: str
    input_values: dict
