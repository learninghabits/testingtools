# Multi-stage build
FROM --platform=linux/arm/v7 mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install .NET 8 runtime manually
RUN apt-get update && apt-get install -y \
    wget \
    tar \
    && wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && ./dotnet-install.sh --runtime dotnet --version 8.0.0 --architecture arm \
    && rm dotnet-install.sh
    
WORKDIR /src

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -r linux-arm --self-contained true -o /app/publish

# Use Debian base for better Raspberry Pi compatibility
FROM --platform=linux/arm/v7 debian:bookworm-slim

# Install OpenCV and camera dependencies
RUN apt-get update && apt-get install -y \
    libopencv-dev \
    python3-opencv \
    v4l-utils \
    # Raspberry Pi specific
    libraspberrypi0 \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

ENV DOTNET_ROOT=/root/.dotnet
ENV PATH=$PATH:/root/.dotnet
ENV LD_LIBRARY_PATH=/opt/vc/lib:/usr/lib/arm-linux-gnueabihf

ENTRYPOINT ["./raspberrypi.tool.qrscanner"]