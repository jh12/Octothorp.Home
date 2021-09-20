FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-amd64 AS publish
ARG RELEASE_VERSION
WORKDIR /src
COPY ["Octothorp.Home/Octothorp.Home.csproj", "Octothorp.Home/"]
RUN dotnet restore "Octothorp.Home/Octothorp.Home.csproj"
COPY . .
WORKDIR "/src/Octothorp.Home"
RUN dotnet publish "Octothorp.Home.csproj" -c Release -p:VersionPrefix=$RELEASE_VERSION -o /app/publish

FROM base AS final

RUN addgroup --system --gid 1000 netcoregroup \
&& adduser --system --uid 1000 --ingroup netcoregroup --shell /bin/sh netcoreuser

WORKDIR /app
COPY --from=publish /app/publish .

USER 1000
ENTRYPOINT ["dotnet", "Octothorp.Home.dll"]