namespace raspberrypi.tool.qrscanner.Services
{
    public class QrScannerService : BackgroundService
    {
        private readonly CameraService _cameraService;
        private readonly HttpService _httpService;
        private readonly HashSet<string> _recentQrCodes;
        private readonly TimeSpan _duplicateWindow = TimeSpan.FromSeconds(10);

        public QrScannerService(CameraService cameraService, HttpService httpService)
        {
            _cameraService = cameraService;
            _httpService = httpService;
            _recentQrCodes = new HashSet<string>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Initializing QR Scanner Service...");

            if (!_cameraService.InitializeCamera())
            {
                Console.WriteLine("Failed to initialize camera. Service stopping.");
                return;
            }

            Console.WriteLine("Camera initialized. Starting QR detection...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var qrCode = _cameraService.DetectQrCode();

                    if (!string.IsNullOrEmpty(qrCode))
                    {
                        if (!IsDuplicate(qrCode))
                        {
                            await _httpService.SendQrDetectionAsync(qrCode);
                            AddToRecent(qrCode);
                        }
                    }

                    await Task.Delay(100, stoppingToken); // Small delay between detections
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Service error: {ex.Message}");
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        private bool IsDuplicate(string qrCode)
        {
            return _recentQrCodes.Contains(qrCode);
        }

        private void AddToRecent(string qrCode)
        {
            _recentQrCodes.Add(qrCode);

            // Remove after duplicate window expires
            Task.Delay(_duplicateWindow).ContinueWith(_ =>
            {
                _recentQrCodes.Remove(qrCode);
            });
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("QR Scanner Service stopping...");
            _cameraService.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}
