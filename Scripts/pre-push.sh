#!/bin/sh

dotnet restore src/Contador.sln
dotnet build src/Contador/Tests/Contador.Tests.csproj
dotnet test src/Contador/Tests/Contador.Tests.csproj