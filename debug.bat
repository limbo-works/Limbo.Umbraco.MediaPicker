@echo off
dotnet build src/Limbo.Umbraco.MediaPicker --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:\nuget\Umbraco13