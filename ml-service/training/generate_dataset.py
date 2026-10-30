"""
Generate synthetic agricultural dataset for training.
Based on soil moisture, temperature, humidity -> water stress classification.

Owner: Abdur Rahman (230241117)
"""

import pandas as pd
import numpy as np
import os

def generate_dataset(n_samples=2000, seed=42):
    np.random.seed(seed)

    soil_moisture = np.random.uniform(5, 95, n_samples)   # %
    temperature   = np.random.uniform(15, 45, n_samples)   # Celsius
    humidity      = np.random.uniform(20, 95, n_samples)   # %

    # Rule-based labels (domain knowledge)
    labels = []
    recommendations = []

    for sm, t, h in zip(soil_moisture, temperature, humidity):
        # High stress: low moisture + high temp + low humidity
        if sm < 30 and t > 35 and h < 40:
            stress = "High"
            rec = "Irrigation Needed"
        elif sm < 30 and (t > 30 or h < 50):
            stress = "High"
            rec = "Irrigation Needed"
        elif sm < 45 and t > 32:
            stress = "Medium"
            rec = "Monitor"
        elif sm < 40:
            stress = "Medium"
            rec = "Monitor"
        elif sm > 70:
            stress = "Low"
            rec = "No Action"
        else:
            stress = "Low"
            rec = "No Action"

        # Add small noise
        r = np.random.random()
        if r < 0.05:  # 5% label flip for realism
            stress = np.random.choice(["Low", "Medium", "High"])
            rec = {"Low": "No Action", "Medium": "Monitor", "High": "Irrigation Needed"}[stress]

        labels.append(stress)
        recommendations.append(rec)

    df = pd.DataFrame({
        "soil_moisture": soil_moisture.round(2),
        "temperature": temperature.round(2),
        "humidity": humidity.round(2),
        "water_stress_risk": labels,
        "irrigation_recommendation": recommendations
    })

    return df


if __name__ == "__main__":
    df = generate_dataset(2000)
    os.makedirs("data", exist_ok=True)
    df.to_csv("data/agrosense_dataset.csv", index=False)
    print(f"Dataset created: {len(df)} rows")
    print(df["water_stress_risk"].value_counts())
    print(df.head())
