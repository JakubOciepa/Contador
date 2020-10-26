#!/bin/sh

dotnet restore src/Contador.sln
dotnet build src/Contador.Web/Server.Tests/Contador.Web.Server.Tests.csproj
dotnet test src/Contador.Web/Server.Tests/Contador.Web.Server.Tests.csproj