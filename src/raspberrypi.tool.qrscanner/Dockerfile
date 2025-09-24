# Multi-stage build
FROM --platform=linux/arm/v7 mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -r linux-arm --self-contained true -o /app/publish

# Use Raspberry Pi compatible base image
FROM --platform=linux/arm/v7 balenalib/raspberrypi3-debian:bookworm

# Install complete OpenCV and dependencies
RUN apt-get update --fix-missing && \
    for package in \
        libopencv-core4.5 \
        libopencv-imgproc4.5 \
        libopencv-imgcodecs4.5 \
        libopencv-videoio4.5 \
        libopencv-objdetect4.5 \
        libopencv-highgui4.5 \
        libtesseract-dev \
        tesseract-ocr \
        v4l-utils \
        libv4l-dev \
        libjpeg-dev \
        libpng-dev \
        libtiff-dev \
        libraspberrypi0 \
        libraspberrypi-dev \
        libgstreamer1.0-0 \
        libgstreamer-plugins-base1.0-0 \
        libgtk-3-0 \
        libsm6 \
        libxext6 \
        libx11-6 \
    ; do \
        apt-get install -y "$package" || echo "Package $package not available, continuing..."; \
    done && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

# Set library path for OpenCV
ENV LD_LIBRARY_PATH=/usr/lib/arm-linux-gnueabihf:/opt/vc/lib

ENTRYPOINT ["./raspberrypi.tool.qrscanner"]