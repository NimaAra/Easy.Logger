@echo off
set releaseVersion=%1

dotnet restore .\Easy.Logger.Extensions
dotnet pack .\Easy.Logger.Extensions\Easy.Logger.Extensions.csproj --output ..\nupkgs --configuration Release /p:Version=%releaseVersion% --include-symbols --include-source