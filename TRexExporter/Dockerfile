#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TrexExporter/TrexExporter.csproj", "TrexExporter/"]
RUN dotnet restore "TrexExporter/TrexExporter.csproj"
COPY . .
WORKDIR "/src/TrexExporter"
RUN dotnet build "TrexExporter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrexExporter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrexExporter.dll"]