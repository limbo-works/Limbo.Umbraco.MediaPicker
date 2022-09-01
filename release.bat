@echo off
dotnet build src/Limbo.Umbraco.MediaPicker --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget