@echo off

set version=%1

dotnet restore .\Easy.Logger.Extensions
dotnet pack .\Easy.Logger.Extensions\Easy.Logger.Extensions.csproj --output ..\nupkgs --configuration Release /p:PackageVersion=%version% --include-symbols --include-source