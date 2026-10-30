import os
import json
import joblib
import numpy as np
from typing import Optional

# Paths
BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
MODELS_DIR = os.path.join(BASE_DIR, "models")

_model = None
_encoder = None
_metadata = None

RECOMMENDATION_MAP = {
    "High":   "Irrigation Needed",
    "Medium": "Monitor",
    "Low":    "No Action"
}

def load_model():
    global _model, _encoder, _metadata

    model_path   = os.path.join(MODELS_DIR, "agrosense_model.pkl")
    encoder_path = os.path.join(MODELS_DIR, "label_encoder.pkl")
    meta_path    = os.path.join(MODELS_DIR, "metadata.json")

    if not os.path.exists(model_path):
        raise FileNotFoundError(
            "Model not found. Run: python training/train.py first."
        )

    _model   = joblib.load(model_path)
    _encoder = joblib.load(encoder_path)

    if os.path.exists(meta_path):
        with open(meta_path) as f:
            _metadata = json.load(f)

    print("[OK] Model loaded successfully.")


def get_model():
    if _model is None:
        load_model()
    return _model, _encoder, _metadata


def build_explanation(feature_importances: dict, soil_moisture: float,
                      temperature: float, humidity: float, risk: str) -> str:
    """Build a human-readable explanation string."""
    top_feature = max(feature_importances, key=feature_importances.get)
    top_weight  = feature_importances[top_feature]

    values = {
        "soil_moisture": soil_moisture,
        "temperature":   temperature,
        "humidity":      humidity
    }

    readable = {
        "soil_moisture": f"Soil Moisture ({soil_moisture:.1f}%)",
        "temperature":   f"Temperature ({temperature:.1f}°C)",
        "humidity":      f"Humidity ({humidity:.1f}%)"
    }

    explanation = (
        f"{readable[top_feature]} is the primary factor "
        f"(importance: {top_weight:.0%}). "
    )

    if risk == "High":
        explanation += "Immediate irrigation is recommended."
    elif risk == "Medium":
        explanation += "Continue monitoring conditions."
    else:
        explanation += "Soil conditions are adequate."

    return explanation


def predict(soil_moisture: float, temperature: float, humidity: float, source: str = "manual") -> dict:
    model, encoder, metadata = get_model()

    X = np.array([[soil_moisture, temperature, humidity]])
    pred_idx    = model.predict(X)[0]
    pred_proba  = model.predict_proba(X)[0]

    risk_label  = encoder.inverse_transform([pred_idx])[0]
    confidence  = float(pred_proba[pred_idx])
    recommendation = RECOMMENDATION_MAP.get(risk_label, "Monitor")

    # Feature importance explanation
    feature_importances = {}
    if metadata and "feature_importances" in metadata:
        feature_importances = metadata["feature_importances"]
    else:
        fi = model.feature_importances_
        feature_importances = {
            "soil_moisture": float(fi[0]),
            "temperature":   float(fi[1]),
            "humidity":      float(fi[2])
        }

    explanation = build_explanation(feature_importances, soil_moisture, temperature, humidity, risk_label)

    return {
        "water_stress_risk":       risk_label,
        "irrigation_recommendation": recommendation,
        "confidence":              round(confidence, 4),
        "explanation":             explanation,
        "source":                  source,
        "input_values": {
            "soil_moisture": soil_moisture,
            "temperature":   temperature,
            "humidity":      humidity
        }
    }
