# AgroSense AI — ML Service (Step 2)
**Owner: Abdur Rahman (230241117)**

## Tech Stack
- Python 3.12
- FastAPI + Uvicorn
- scikit-learn (Random Forest) + XGBoost
- SHAP for explainability
- Pydantic for validation

## Setup

```bash
cd ml-service

# Create virtual environment
python -m venv venv
venv\Scripts\activate   # Windows

# Install dependencies
pip install -r requirements.txt

# Generate dataset & train model
python training/generate_dataset.py
python training/train.py

# Start the inference server
uvicorn app.main:app --reload --port 8000
```

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | /health | Check if model is loaded |
| GET | /model-info | Model metadata & feature importances |
| POST | /predict | Run prediction |

## Sample Request

```json
POST http://localhost:8000/predict
{
  "soil_moisture": 24.0,
  "temperature": 32.0,
  "humidity": 65.0,
  "source": "manual"
}
```

## Sample Response

```json
{
  "water_stress_risk": "Medium",
  "irrigation_recommendation": "Monitor",
  "confidence": 0.78,
  "explanation": "Soil Moisture (24.0%) is the primary factor (importance: 62%). Continue monitoring conditions.",
  "source": "manual",
  "input_values": {
    "soil_moisture": 24.0,
    "temperature": 32.0,
    "humidity": 65.0
  }
}
```
