"""
AgroSense AI — FastAPI ML Inference Service
Owner: Abdur Rahman (230241117)

Endpoints:
  POST /predict     — Run prediction
  GET  /health      — Health check
  GET  /model-info  — Model metadata
"""

from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from contextlib import asynccontextmanager
from app.schemas import PredictRequest, PredictResponse
from app import model as ml_model
import os

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Load model on startup
    try:
        ml_model.load_model()
    except FileNotFoundError as e:
        print(f"⚠️  Warning: {e}")
        print("   Run `python training/train.py` to train the model first.")
    yield


app = FastAPI(
    title="AgroSense AI — ML Service",
    description="Crop Water-Stress Risk Prediction API",
    version="1.0.0",
    lifespan=lifespan
)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)


@app.get("/health")
def health_check():
    model_loaded = ml_model._model is not None
    return {
        "status": "ok" if model_loaded else "model_not_loaded",
        "model_loaded": model_loaded
    }


@app.get("/model-info")
def model_info():
    _, _, metadata = ml_model.get_model()
    return metadata or {"error": "No metadata available"}


@app.post("/predict", response_model=PredictResponse)
def predict(request: PredictRequest):
    try:
        result = ml_model.predict(
            soil_moisture=request.soil_moisture,
            temperature=request.temperature,
            humidity=request.humidity,
            source=request.source or "manual"
        )
        return result
    except FileNotFoundError as e:
        raise HTTPException(status_code=503, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Prediction error: {str(e)}")
