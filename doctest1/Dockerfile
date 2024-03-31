FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

RUN apt-get update \
    && apt-get install -y apt-transport-https ca-certificates curl gnupg-agent software-properties-common \
    && curl -fsSL https://download.docker.com/linux/ubuntu/gpg | apt-key add - \
    && add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" \
    && apt-get update \
    && apt-get install -y docker-ce-cli

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "doctest1/doctest1.csproj"
WORKDIR "/src/doctest1"
RUN dotnet build "doctest1.csproj" -c Release -o /app/build

COPY --from=build /usr/bin/docker /usr/bin/docker
VOLUME /var/run/docker.sock:/var/run/docker.sock

FROM build AS publish
WORKDIR "/src/doctest1"
RUN dotnet publish "doctest1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "doctest1.dll"]
