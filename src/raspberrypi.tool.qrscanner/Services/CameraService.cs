using OpenCvSharp;

namespace raspberrypi.tool.qrscanner.Services
{
    public class CameraService : IDisposable
    {
        private VideoCapture? _capture;
        private QRCodeDetector _qrDetector;
        private bool _isInitialized = false;

        public CameraService()
        {
            _qrDetector = new QRCodeDetector();
        }

        public bool InitializeCamera()
        {
            try
            {
                // Try different camera indices (0 is usually the default)
                _capture = new VideoCapture(0);

                if (!_capture.IsOpened())
                {
                    // Try with backend preference for Raspberry Pi camera
                    _capture = new VideoCapture(0, VideoCaptureAPIs.V4L2);
                }

                if (_capture.IsOpened())
                {
                    // Set camera properties for Raspberry Pi
                    _capture.Set(VideoCaptureProperties.FrameWidth, 640);
                    _capture.Set(VideoCaptureProperties.FrameHeight, 480);
                    _capture.Set(VideoCaptureProperties.Fps, 30);

                    _isInitialized = true;
                    return true;
                }

                Console.WriteLine("Failed to open camera");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Camera initialization error: {ex.Message}");
                return false;
            }
        }

        public string? DetectQrCode()
        {
            if (!_isInitialized || _capture == null)
                return null;

            try
            {
                using var frame = new Mat();
                if (_capture.Read(frame) && !frame.Empty())
                {
                    string? result = _qrDetector.DetectAndDecode(frame, out var points);

                    if (!string.IsNullOrEmpty(result) && points != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"QR detection error: {ex.Message}");
            }

            return null;
        }

        public void Dispose()
        {
            _capture?.Release();
            _capture?.Dispose();
            _qrDetector?.Dispose();
        }
    }
}
