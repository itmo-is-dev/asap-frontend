FROM mcr.microsoft.com/dotnet/sdk:7.0.203 AS build
WORKDIR /source
COPY . .

ARG GITHUB_TOKEN
ARG GITHUB_USERNAME
RUN dotnet nuget update source github --username $GITHUB_USERNAME --password $GITHUB_TOKEN --store-password-in-clear-text
RUN dotnet restore "src/Itmo.Dev.Asap.Frontend/Server/Itmo.Dev.Asap.Frontend.Server.csproj"

FROM build AS publish
WORKDIR "/source/src/Itmo.Dev.Asap.Frontend/Server"
RUN dotnet publish "Itmo.Dev.Asap.Frontend.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0.5 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Itmo.Dev.Asap.Frontend.Server.dll"]
