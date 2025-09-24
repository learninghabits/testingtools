
# Install .NET 8 runtime manually
# Download the script first
sudo apt-get update --fix-missing && apt-get install -y wget tar \
    && wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x dotnet-install.sh

# Install .NET 8 runtime and clean up
./dotnet-install.sh --runtime dotnet --version 8.0.0
rm dotnet-install.sh