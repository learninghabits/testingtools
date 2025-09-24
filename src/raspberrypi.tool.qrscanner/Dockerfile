# Multi-stage build
FROM --platform=linux/arm/v7 mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -r linux-arm --self-contained true -o /app/publish

# Use Raspberry Pi OS base image for proper Raspberry Pi compatibility
FROM --platform=linux/arm/v7 balenalib/raspberrypi3-debian:bookworm

# Install dependencies from proper Raspberry Pi repositories
RUN apt-get update && apt-get install -y \
    libopencv-dev \
    python3-opencv \
    v4l-utils \
    libraspberrypi-bin \
    libraspberrypi-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["./raspberrypi.tool.qrscanner"]