#!/bin/bash
echo "Testing camera on Raspberry Pi OS..."

# Test legacy camera stack
echo "1. Testing legacy camera stack..."
if command -v raspistill &> /dev/null; then
    raspistill -o test_legacy.jpg -t 1000
    if [ -f test_legacy.jpg ]; then
        echo "✓ Legacy camera stack works"
    else
        echo "✗ Legacy camera stack failed"
    fi
fi

# Test new camera stack
echo "2. Testing new camera stack..."
if command -v libcamera-still &> /dev/null; then
    libcamera-still -o test_libcamera.jpg -t 1000
    if [ -f test_libcamera.jpg ]; then
        echo "✓ libcamera stack works"
    else
        echo "✗ libcamera stack failed"
    fi
fi

# Test OpenCV camera access
echo "3. Testing OpenCV camera access..."
python3 -c "
import cv2
import sys
cap = cv2.VideoCapture(0)
if cap.isOpened():
    ret, frame = cap.read()
    if ret:
        cv2.imwrite('test_opencv.jpg', frame)
        print('✓ OpenCV camera access works')
    else:
        print('✗ OpenCV could not read frame')
    cap.release()
else:
    print('✗ OpenCV could not open camera')
"

echo "Camera test completed!"