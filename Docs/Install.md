# Install dotnet
-  wget https://download.visualstudio.microsoft.com/download/pr/9d2abf34-b484-46ab-8e3b-504b0057827b/7266d743d6441c1f80510a50c17491dc/aspnetcore-runtime-5.0.6-linux-arm.tar.gz

- mkdir $HOME/dotnet
- tar zxf dotnet-sdk-5.0.300-linux-arm.tar.gz -C $HOME/dotnet
- tar zxf aspnetcore-runtime-5.0.6-linux-arm.tar.gz -C $HOME/dotnet

- add following to ~/.bashrc
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet