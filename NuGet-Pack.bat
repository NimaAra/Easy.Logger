@echo off

set version=%1

dotnet restore .\Easy.Logger
dotnet pack .\Easy.Logger\Easy.Logger.csproj --output ..\nupkgs --configuration Release /p:PackageVersion=%version% --include-symbols --include-source