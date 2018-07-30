@echo off
set releaseVersion=%1

dotnet restore .\Easy.Logger.Extensions.Microsoft
dotnet pack .\Easy.Logger.Extensions.Microsoft\Easy.Logger.Extensions.Microsoft.csproj --output ..\nupkgs --configuration Release /p:Version=%releaseVersion% --include-symbols --include-source