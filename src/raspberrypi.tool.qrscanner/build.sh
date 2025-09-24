#!/bin/bash
set -e

echo "Running docker compose build ..."
docker compose build
echo "Running docker compose up ..."
docker compose up -d
echo "Waiting for the container to be ready ..."
sleep 10
echo "Reading logs"
docker logs -f qr-scanner-service