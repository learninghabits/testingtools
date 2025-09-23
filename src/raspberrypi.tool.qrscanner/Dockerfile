FROM mcr.microsoft.com/dotnet/runtime:8.0

# Install system OpenCV and dependencies
RUN apt-get update && apt-get install -y \
    libopencv-core4.5 \
    libopencv-imgproc4.5 \
    libopencv-imgcodecs4.5 \
    libopencv-videoio4.5 \
    libopencv-highgui4.5 \
    python3-opencv \
    v4l-utils \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Create symbolic links that OpenCvSharp expects
RUN ln -sf /usr/lib/aarch64-linux-gnu/libopencv_core.so.4.5 /usr/lib/aarch64-linux-gnu/libopencv_core.so && \
    ln -sf /usr/lib/aarch64-linux-gnu/libopencv_imgproc.so.4.5 /usr/lib/aarch64-linux-gnu/libopencv_imgproc.so && \
    ln -sf /usr/lib/aarch64-linux-gnu/libopencv_imgcodecs.so.4.5 /usr/lib/aarch64-linux-gnu/libopencv_imgcodecs.so && \
    ln -sf /usr/lib/aarch64-linux-gnu/libopencv_videoio.so.4.5 /usr/lib/aarch64-linux-gnu/libopencv_videoio.so && \
    ln -sf /usr/lib/aarch64-linux-gnu/libopencv_highgui.so.4.5 /usr/lib/aarch64-linux-gnu/libopencv_highgui.so

WORKDIR /app
COPY publish/ .

# Set library path
ENV LD_LIBRARY_PATH=/usr/lib/aarch64-linux-gnu

ENTRYPOINT ["./raspberrypi.tool.qrscanner"]