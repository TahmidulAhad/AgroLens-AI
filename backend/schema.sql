-- AgroSense AI - NeonDB / PostgreSQL Schema
-- Run this in your NeonDB SQL console

CREATE TABLE IF NOT EXISTS devices (
    id SERIAL PRIMARY KEY,
    device_id VARCHAR(100) UNIQUE NOT NULL,
    name VARCHAR(200) NOT NULL,
    location VARCHAR(200),
    is_active BOOLEAN DEFAULT TRUE,
    registered_at TIMESTAMPTZ DEFAULT NOW(),
    last_seen TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS sensor_readings (
    id SERIAL PRIMARY KEY,
    soil_moisture FLOAT NOT NULL CHECK (soil_moisture BETWEEN 0 AND 100),
    temperature FLOAT NOT NULL,
    humidity FLOAT NOT NULL CHECK (humidity BETWEEN 0 AND 100),
    source VARCHAR(20) DEFAULT 'iot',
    device_id VARCHAR(100) DEFAULT 'unknown',
    timestamp TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS predictions (
    id SERIAL PRIMARY KEY,
    soil_moisture FLOAT NOT NULL,
    temperature FLOAT NOT NULL,
    humidity FLOAT NOT NULL,
    water_stress_risk VARCHAR(20),   -- Low / Medium / High
    irrigation_recommendation VARCHAR(50), -- Irrigation Needed / Monitor / No Action
    confidence FLOAT,
    explanation TEXT,
    source VARCHAR(20) DEFAULT 'manual',
    timestamp TIMESTAMPTZ DEFAULT NOW()
);

-- Indexes for faster queries
CREATE INDEX IF NOT EXISTS idx_sensor_readings_timestamp ON sensor_readings(timestamp DESC);
CREATE INDEX IF NOT EXISTS idx_predictions_timestamp ON predictions(timestamp DESC);
CREATE INDEX IF NOT EXISTS idx_sensor_readings_device ON sensor_readings(device_id);

