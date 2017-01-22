@echo off
dotnet restore src\Easy.Logger
dotnet pack --output NuGet --configuration Release src\Easy.Logger