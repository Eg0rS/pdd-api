FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["pdd-backend/pdd-backend.csproj", "pdd-backend/"]
RUN dotnet restore "pdd-backend/pdd-backend.csproj"
COPY . .
WORKDIR "/src/pdd-backend"
RUN dotnet build "pdd-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pdd-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pdd-backend.dll"]
