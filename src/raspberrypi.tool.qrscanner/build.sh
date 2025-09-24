#!/bin/bash
set -e

echo "Building for Raspberry Pi OS (32-bit ARM)..."

# Clean previous builds
echo "Step 1: Cleaning previous builds..."
dotnet clean
rm -rf ./publish

# Build for ARM32
echo "Step 2: Building .NET application for linux-arm..."
dotnet publish -c Release -r linux-arm --self-contained true -o ./publish

echo "Step 3: Verifying build..."
ls -la ./publish/
file ./publish/QrScannerService 2>/dev/null || echo "Checking DLL..."
file ./publish/*.dll 2>/dev/null || true

echo "Step 4: Building Docker image for ARM32..."
docker build --platform linux/arm/v7 -t qr-scanner-service .

echo "Build completed successfully!"
echo "Run: docker compose up -d"