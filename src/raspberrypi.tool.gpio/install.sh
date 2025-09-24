
# Install .NET 8 runtime manually
# Download the script first
RUN apt-get update --fix-missing && apt-get install -y wget tar \
    && wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x dotnet-install.sh

# Execute the script in a separate RUN command
RUN ./dotnet-install.sh --runtime dotnet --version 8.0.0 --architecture arm \
    && rm dotnet-install.sh
    