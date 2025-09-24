# Update package list and install dependencies
sudo apt-get update --fix-missing
sudo apt-get install -y wget tar

# Download .NET install script
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh

# Install .NET 8 runtime
./dotnet-install.sh --runtime dotnet --version 8.0.0

# Clean up
rm dotnet-install.sh

# Register dotnet command in PATH
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc

# Verify installation
dotnet --version