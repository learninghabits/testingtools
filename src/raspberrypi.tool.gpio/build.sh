# Build the application
dotnet build

# Publish for deployment
dotnet publish -c Release -r linux-arm --self-contained false

# Run the application
cd bin/Release/net6.0/linux-arm/publish ./raspberrypi.tool.gpio