FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "doctest1/doctest1.csproj"
WORKDIR "/src/doctest1"
RUN dotnet build "doctest1.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/doctest1"
RUN dotnet publish "doctest1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "doctest1.dll"]
