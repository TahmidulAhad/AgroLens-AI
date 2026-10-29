# 🌾 AgroLens-AI (AgroSense AI)
### Hybrid IoT & Web-Based Intelligent Crop Monitoring and Prediction System

An end-to-end precision agriculture platform combining real-time IoT soil and environmental telemetry with Machine Learning inference to optimize irrigation and crop health management.

---

## 👥 Project Team & Modular Structure

| Step | Team Member | Student ID | Domain / Module | Tech Stack & Architecture | Status |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Step 1** | **Md. Tahmidul Alam Ahad** | **230241121** | **Backend API & Ingestion Engine** | **Clean Architecture** (Domain, Application, Infrastructure, API) in ASP.NET Core 10, EF Core, NeonDB | ✅ **Built & Tested (0 Errors)** |
| **Step 2** | **Abdur Rahman** | **230241117** | **Machine Learning & Inference Engine** | Python 3.12, scikit-learn (RandomForest 94.75% acc), XGBoost, FastAPI, SHAP | ✅ **Model Trained & Exported** |
| **Step 3** | **Mohammad Tareq Aziz** | **230241097** | **Interactive Web Dashboard** | React 19, Vite, Tailwind CSS v4, Recharts, Lucide | ✅ **Built & Tested** |

---

## 🏛️ Backend Clean Architecture Layers (Step 1)

```
AgroSense (Backend Solution)
│
├── 1. AgroSense.Domain          (Core entities: SensorReading, Prediction, Device)
│      ▲
├── 2. AgroSense.Application     (DTOs, Use Case Services, IAppDbContext, IMLService)
│      ▲
├── 3. AgroSense.Infrastructure  (EF Core Npgsql Persistence, FastAPI HTTP Client)
│      ▲
└── 4. AgroSense.API             (REST Controllers, Swagger, CORS, DI Bootstrapper)
```

---

## 🏗️ System Architecture

```
[ ESP32 IoT Sensors ] ──(HTTP POST)──┐
                                     ▼
[ Web Simulation UI ] ──(HTTP POST)──► [ AgroSense.API ] (Step 1)
                                             │
                       ┌─────────────────────┴─────────────────────┐
                       ▼                                           ▼
            [ NeonDB / PostgreSQL ]                      [ FastAPI ML Service ] (Step 2)
            - sensor_readings                            - Random Forest (94.75% acc)
            - predictions                                - SHAP Explainability
            - devices
                       ▲                                           │
                       └─────────────────────┬─────────────────────┘
                                             ▼
                                  [ React Web Dashboard ] (Step 3)
```

---

## 📁 Repository Structure

```
AgroLens_AI/
├── backend/                              # Step 1: Md. Tahmidul Alam Ahad (230241121)
│   ├── AgroSense.slnx                    # .NET Solution File
│   ├── src/
│   │   ├── AgroSense.Domain/             # Layer 1: Entities (SensorReading, Prediction, Device)
│   │   ├── AgroSense.Application/        # Layer 2: DTOs, Use Cases, Interfaces (ISensorService, etc.)
│   │   ├── AgroSense.Infrastructure/     # Layer 3: DB Context, Npgsql EF Core, ML HTTP Client
│   │   └── AgroSense.API/                # Layer 4: Controllers, Swagger, Program.cs
│   ├── schema.sql                        # NeonDB Database DDL
│   └── README.md
│
├── ml-service/                           # Step 2: Abdur Rahman (230241117)
│   ├── app/
│   │   ├── main.py                       # FastAPI server (POST /predict, GET /health)
│   │   ├── model.py                      # Model loader & explainability engine
│   │   └── schemas.py                    # Pydantic validation schemas
│   ├── training/
│   │   ├── generate_dataset.py           # Agricultural synthetic dataset generator
│   │   └── train.py                      # Model training & export (94.75% accuracy)
│   ├── models/
│   │   ├── agrosense_model.pkl           # Trained Random Forest artifact
│   │   ├── label_encoder.pkl             # Class encoder
│   │   └── metadata.json                 # Feature importances & metrics
│   ├── requirements.txt
│   └── README.md
│
├── frontend/                             # Step 3: Mohammad Tareq Aziz (230241097)
│   ├── src/
│   │   ├── components/
│   │   │   ├── SensorCard.jsx            # Live IoT Sensor Card
│   │   │   ├── PredictForm.jsx           # Manual Input Simulation Sliders
│   │   │   ├── SensorChart.jsx           # Recharts Time-series Trends
│   │   │   ├── AlertBanner.jsx           # Water-Stress Alerts
│   │   │   └── PredictionHistory.jsx     # Past Prediction Records
│   │   ├── api/agrosense.js              # Centralized Axios Client
│   │   └── App.jsx                       # Main View
│   ├── package.json
│   ├── vite.config.js
│   └── README.md
│
└── README.md
```

---

## 🚀 Running the Project

### 1. Step 1: Clean Architecture Backend API (Md. Tahmidul Alam Ahad)
```bash
cd backend/src/AgroSense.API
dotnet run
```
- Swagger API Docs: `http://localhost:5000/swagger`

### 2. Step 2: ML Inference Engine (Abdur Rahman)
```bash
cd ml-service
uvicorn app.main:app --reload --port 8000
```
- FastAPI Docs: `http://localhost:8000/docs`

### 3. Step 3: React Dashboard (Mohammad Tareq Aziz)
```bash
cd frontend
pnpm run dev
```
- Web Application: `http://localhost:5173`
