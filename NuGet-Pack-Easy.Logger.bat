@echo off
set releaseVersion=%1

dotnet restore .\Easy.Logger
dotnet pack .\Easy.Logger\Easy.Logger.csproj --output .\nupkgs --configuration Release /p:Version=%releaseVersion% --include-symbols --include-source