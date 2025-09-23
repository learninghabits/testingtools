using raspberrypi.tool.qrscanner.Models;
using System.Text;
using System.Text.Json;

namespace raspberrypi.tool.qrscanner.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string[] _targetUrls = {
            "https://webhook.site/YOUR_WEBHOOK_ID_1",
            "https://webhook.site/YOUR_WEBHOOK_ID_2",
            "https://webhook.site/YOUR_WEBHOOK_ID_3"
        };

        public HttpService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<bool> SendQrDetectionAsync(string qrCode)
        {
            var random = new Random();
            var targetUrl = _targetUrls[random.Next(_targetUrls.Length)];

            var payload = new QrDetectionPayload
            {
                QrCode = qrCode,
                Timestamp = DateTime.UtcNow,
                DeviceId = "raspberry-pi-3"
            };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(targetUrl, content);

                Console.WriteLine($"QR detected: {qrCode}");
                Console.WriteLine($"Sent to: {targetUrl}");
                Console.WriteLine($"Response: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                return false;
            }
        }
    }
}
