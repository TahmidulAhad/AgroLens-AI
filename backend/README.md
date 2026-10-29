# AgroSense AI — Backend (Clean Architecture)
**Owner: Md. Tahmidul Alam Ahad (230241121)**

## 🏛️ Clean Architecture Structure

```
backend/
├── AgroSense.slnx
├── src/
│   ├── AgroSense.Domain/            # 1. Domain Layer (Core Entities & Rules)
│   │   └── Entities/
│   │       ├── SensorReading.cs
│   │       ├── Prediction.cs
│   │       └── Device.cs
│   │
│   ├── AgroSense.Application/       # 2. Application Layer (Use Cases & Contracts)
│   │   ├── Common/Interfaces/
│   │   │   ├── IAppDbContext.cs
│   │   │   └── IMLService.cs
│   │   ├── DTOs/
│   │   │   ├── SensorReadingDto.cs
│   │   │   ├── PredictRequestDto.cs
│   │   │   ├── PredictionResponseDto.cs
│   │   │   ├── DeviceDto.cs
│   │   │   └── MLPredictionResultDto.cs
│   │   ├── Interfaces/
│   │   │   ├── ISensorService.cs
│   │   │   ├── IPredictionService.cs
│   │   │   └── IDeviceService.cs
│   │   ├── Services/
│   │   │   ├── SensorService.cs
│   │   │   ├── PredictionService.cs
│   │   │   └── DeviceService.cs
│   │   └── DependencyInjection.cs
│   │
│   ├── AgroSense.Infrastructure/    # 3. Infrastructure Layer (Database & External APIs)
│   │   ├── Persistence/
│   │   │   └── AppDbContext.cs      # EF Core + Npgsql (NeonDB)
│   │   ├── Services/
│   │   │   └── MLService.cs         # FastAPI client via HttpClient
│   │   └── DependencyInjection.cs
│   │
│   └── AgroSense.API/               # 4. Presentation / API Layer
│       ├── Controllers/
│       │   ├── SensorDataController.cs
│       │   ├── PredictController.cs
│       │   └── DevicesController.cs
│       ├── Program.cs
│       └── appsettings.json
│
├── schema.sql
└── README.md
```

## 🔄 Dependency Rule
- **Domain**: Independent. Zero external project dependencies.
- **Application**: Depends only on **Domain**.
- **Infrastructure**: Depends on **Application** and implements persistence/external services.
- **API**: Presentation layer. Depends on **Application** & **Infrastructure** to configure DI and expose REST endpoints.

## 🚀 Running the Backend

```bash
cd backend/src/AgroSense.API
dotnet run
```

Swagger Documentation will be available at: `http://localhost:5000/swagger`
