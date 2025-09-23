FROM mcr.microsoft.com/dotnet/runtime:8.0

# Install only essential dependencies for OpenCV and camera
RUN apt-get update && apt-get install -y \
    libgdiplus \
    libc6-dev \
    libgstreamer1.0-0 \
    gstreamer1.0-plugins-base \
    gstreamer1.0-plugins-good \
    gstreamer1.0-plugins-bad \
    gstreamer1.0-plugins-ugly \
    gstreamer1.0-libav \
    gstreamer1.0-tools \
    gstreamer1.0-x \
    gstreamer1.0-alsa \
    libopencv-dev \
    python3-opencv \
    v4l-utils \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Create app directory
WORKDIR /app

# Copy published application
COPY publish/ .

# Set environment variables for camera access
ENV LD_LIBRARY_PATH=/usr/lib/aarch64-linux-gnu
ENV DISPLAY=:0

ENTRYPOINT ["./raspberrypi.tool.qrscanner"]