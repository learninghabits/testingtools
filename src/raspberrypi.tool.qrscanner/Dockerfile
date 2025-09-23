FROM mcr.microsoft.com/dotnet/runtime:10.0

# Install dependencies for OpenCV and camera
RUN apt-get update && apt-get install -y \
    libgdiplus \
    libc6-dev \
    libgstreamer1.0-0 \
    gstreamer1.0-plugins-base \
    gstreamer1.0-plugins-good \
    gstreamer1.0-plugins-bad \
    gstreamer1.0-plugins-ugly \
    gstreamer1.0-libav \
    gstreamer1.0-doc \
    gstreamer1.0-tools \
    gstreamer1.0-x \
    gstreamer1.0-alsa \
    gstreamer1.0-gl \
    gstreamer1.0-gtk3 \
    gstreamer1.0-qt5 \
    gstreamer1.0-pulseaudio \
    libopencv-dev \
    python3-opencv \
    && rm -rf /var/lib/apt/lists/*

# Create app directory
WORKDIR /app

# Copy published application
COPY bin/Release/net8.0/linux-arm64/publish/ .

# Set environment variables for camera access
ENV LD_LIBRARY_PATH=/usr/lib/aarch64-linux-gnu
ENV DISPLAY=:0

# Enable camera device access
VOLUME /dev/video0:/dev/video0

ENTRYPOINT ["./raspberrypi.tool.qrscanner"]