@echo off
set releaseVersion=%1

dotnet restore .\Easy.Logger.Interfaces
dotnet pack .\Easy.Logger.Interfaces\Easy.Logger.Interfaces.csproj --output .\nupkgs --configuration Release /p:Version=%releaseVersion% --include-symbols --include-source