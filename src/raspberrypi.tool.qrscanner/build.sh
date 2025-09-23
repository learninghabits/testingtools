#!/bin/bash
set -e

echo "Building Docker image (includes .NET build)..."
docker build -t qr-scanner-service .

echo "Build completed successfully!"
echo "You can now run: docker compose up -d"