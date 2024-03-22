FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "pdd-backend/pdd-backend.csproj"
WORKDIR "/src/pdd-backend"
RUN dotnet build "pdd-backend.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/pdd-backend"
RUN dotnet publish "pdd-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pdd-backend.dll"]