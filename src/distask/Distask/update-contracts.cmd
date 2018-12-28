@echo off
echo Updating contract files...
"%UserProfile%\.nuget\packages\Grpc.Tools\1.17.1\tools\windows_x64\protoc.exe" -I..\..\proto --csharp_out Contracts --grpc_out Contracts ..\..\proto\distask.proto --plugin=protoc-gen-grpc="%UserProfile%\.nuget\packages\Grpc.Tools\1.17.1\tools\windows_x64\grpc_csharp_plugin.exe
echo Done!
