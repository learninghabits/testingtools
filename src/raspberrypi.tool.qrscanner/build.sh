#!/bin/bash

# Exit on error
set -e

echo "Building .NET application..."
# Clean previous builds
dotnet clean

# Create the publish directory with the correct runtime
dotnet publish -c Release -r linux-arm64 --self-contained true -o ./publish

echo "Verifying publish directory..."
ls -la ./publish/

echo "Building Docker image..."
# Build the Docker image using the current directory context
docker build -t qr-scanner-service .

echo "Build completed successfully!"
echo "You can now run: docker compose up -d"