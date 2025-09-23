#!/bin/bash

# Build the .NET application
echo "Building .NET application..."
dotnet publish -c Release -r linux-arm64 --self-contained true

# Build Docker image
echo "Building Docker image..."
docker compose build

echo "Build completed successfully!"