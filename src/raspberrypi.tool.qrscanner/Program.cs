using raspberrypi.tool.qrscanner.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<QrScannerService>();
builder.Services.AddSingleton<CameraService>();
builder.Services.AddSingleton<HttpService>();

var app = builder.Build();

app.Run();