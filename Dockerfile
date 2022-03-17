#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 8088

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build ./TRexExporter/TrexExporter.csproj -c Release -o /app/build -r linux-x64 --self-contained true

FROM build AS publish
RUN dotnet publish ./TRexExporter/TrexExporter.csproj -c Release -o /app/publish -r linux-x64 --self-contained true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrexExporter.dll"]