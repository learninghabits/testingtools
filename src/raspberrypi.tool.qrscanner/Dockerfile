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
# Install complete OpenCV and dependencies with alternatives
RUN apt-get update --fix-missing && \
    # Function to install with fallbacks
    install_pkg() {
        local pkg=$1
        shift
        local alternatives=("$@")
        
        if apt-get install -y "$pkg"; then
            echo "✓ Installed $pkg"
            return 0
        fi
        
        for alt in "${alternatives[@]}"; do
            if [ -n "$alt" ] && apt-get install -y "$alt"; then
                echo "✓ Installed alternative $alt instead of $pkg"
                return 0
            fi
        done
        
        echo "⚠ Could not install $pkg or alternatives, continuing..."
        return 0
    } && \
    
    # Install packages with alternatives
    install_pkg libopencv-core4.5 "libopencv-core-dev" "opencv-data" && \
    install_pkg libopencv-imgproc4.5 "libopencv-imgproc-dev" "" && \
    install_pkg libopencv-imgcodecs4.5 "libopencv-imgcodecs-dev" "" && \
    install_pkg libopencv-videoio4.5 "libopencv-videoio-dev" "" && \
    install_pkg libopencv-objdetect4.5 "libopencv-objdetect-dev" "" && \
    install_pkg libopencv-highgui4.5 "libopencv-highgui-dev" "" && \
    install_pkg libtesseract-dev "tesseract-ocr" "libtesseract4" && \
    install_pkg libraspberrypi0 "libraspberrypi-bin" "" && \
    install_pkg libraspberrypi-dev "" "" && \
    
    # Install essential packages (should work)
    apt-get install -y \
        v4l-utils \
        libv4l-dev \
        libjpeg-dev \
        libpng-dev \
        libtiff-dev \
        libgstreamer1.0-0 \
        libgstreamer-plugins-base1.0-0 \
        libgtk-3-0 \
        libsm6 \
        libxext6 \
        libx11-6 \
    && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

# Set library path for OpenCV
ENV LD_LIBRARY_PATH=/usr/lib/arm-linux-gnueabihf:/opt/vc/lib

ENTRYPOINT ["./raspberrypi.tool.qrscanner"]