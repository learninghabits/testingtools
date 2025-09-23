# Multi-stage build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -r linux-arm64 --self-contained true -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0

# Install only essential dependencies for OpenCV and camera
RUN apt-get update && apt-get install -y \
    libopencv-dev \
    python3-opencv \
    v4l-utils \
    libgdiplus \
    libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy from build stage
COPY --from=build /app/publish .

#ENTRYPOINT ["dotnet", "raspberrypi.tool.qrscanner.dll"]
ENTRYPOINT ["./raspberrypi.tool.qrscanner"]