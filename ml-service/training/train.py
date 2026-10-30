"""
Train the AgroSense AI water-stress classification model.
Uses Random Forest + XGBoost comparison, saves best model.

Owner: Abdur Rahman (230241117)
"""

import os
import sys
import pandas as pd
import numpy as np
import joblib
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.preprocessing import LabelEncoder
from sklearn.metrics import classification_report, confusion_matrix, accuracy_score

# Try XGBoost; fallback to RF if not installed
try:
    from xgboost import XGBClassifier
    XGBOOST_AVAILABLE = True
except ImportError:
    XGBOOST_AVAILABLE = False
    print("XGBoost not available, using Random Forest only.")

# Add parent dir to path
sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))


def train():
    # ── Load dataset ─────────────────────────────────────────────────
    dataset_path = os.path.join(os.path.dirname(__file__), "data", "agrosense_dataset.csv")
    if not os.path.exists(dataset_path):
        print("Dataset not found. Running generator...")
        from generate_dataset import generate_dataset
        df = generate_dataset(2000)
        os.makedirs(os.path.join(os.path.dirname(__file__), "data"), exist_ok=True)
        df.to_csv(dataset_path, index=False)
    else:
        df = pd.read_csv(dataset_path)

    print(f"Loaded dataset: {len(df)} rows")
    print(df["water_stress_risk"].value_counts())

    # ── Features & Labels ────────────────────────────────────────────
    features = ["soil_moisture", "temperature", "humidity"]
    X = df[features].values

    le = LabelEncoder()
    y = le.fit_transform(df["water_stress_risk"])  # High=0, Low=1, Medium=2

    print(f"\nClasses: {le.classes_}")

    # ── Train/Test Split ─────────────────────────────────────────────
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )

    # ── Model Training ───────────────────────────────────────────────
    print("\n--- Training Random Forest ---")
    rf = RandomForestClassifier(
        n_estimators=200,
        max_depth=10,
        random_state=42,
        class_weight="balanced"
    )
    rf.fit(X_train, y_train)
    rf_acc = accuracy_score(y_test, rf.predict(X_test))
    print(f"Random Forest Accuracy: {rf_acc:.4f}")
    print(classification_report(y_test, rf.predict(X_test), target_names=le.classes_))

    best_model = rf
    best_name = "RandomForest"

    if XGBOOST_AVAILABLE:
        print("\n--- Training XGBoost ---")
        xgb = XGBClassifier(
            n_estimators=200,
            max_depth=6,
            learning_rate=0.1,
            use_label_encoder=False,
            eval_metric="mlogloss",
            random_state=42
        )
        xgb.fit(X_train, y_train)
        xgb_acc = accuracy_score(y_test, xgb.predict(X_test))
        print(f"XGBoost Accuracy: {xgb_acc:.4f}")
        print(classification_report(y_test, xgb.predict(X_test), target_names=le.classes_))

        if xgb_acc > rf_acc:
            best_model = xgb
            best_name = "XGBoost"

    print(f"\n[OK] Best model: {best_name}")

    # ── Feature Importance ───────────────────────────────────────────
    importances = best_model.feature_importances_
    for feat, imp in zip(features, importances):
        print(f"  {feat}: {imp:.4f}")

    # ── Save Model ───────────────────────────────────────────────────
    models_dir = os.path.join(os.path.dirname(os.path.dirname(__file__)), "models")
    os.makedirs(models_dir, exist_ok=True)

    joblib.dump(best_model, os.path.join(models_dir, "agrosense_model.pkl"))
    joblib.dump(le, os.path.join(models_dir, "label_encoder.pkl"))

    # Save feature importances as metadata
    metadata = {
        "features": features,
        "classes": list(le.classes_),
        "feature_importances": dict(zip(features, importances.tolist())),
        "model_type": best_name,
        "accuracy": float(rf_acc if best_name == "RandomForest" else xgb_acc)
    }
    import json
    with open(os.path.join(models_dir, "metadata.json"), "w") as f:
        json.dump(metadata, f, indent=2)

    print(f"\n[OK] Model saved to models/agrosense_model.pkl")
    print(f"[OK] Encoder saved to models/label_encoder.pkl")
    print(f"[OK] Metadata saved to models/metadata.json")

    return best_model, le


if __name__ == "__main__":
    train()
