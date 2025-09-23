namespace raspberrypi.tool.qrscanner.Models
{
    public class QrDetectionPayload
    {
        public string QrCode { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string DeviceId { get; set; } = "raspberry-pi-3";
    }
}
