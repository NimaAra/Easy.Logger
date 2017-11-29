@echo off

set version=%1

dotnet restore .\Easy.Logger.Interfaces
dotnet pack .\Easy.Logger.Interfaces\Easy.Logger.Interfaces.csproj --output ..\nupkgs --configuration Release /p:PackageVersion=%version% --include-symbols --include-source