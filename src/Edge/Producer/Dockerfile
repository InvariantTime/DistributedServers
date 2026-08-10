
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY "Edge/Producer/" ./Edge/Producer/

RUN dotnet publish "Edge/Producer/DistributedServers.Edge.Producer/DistributedServers.Edge.Producer.csproj" -c Release  -o /.out

FROM mcr.microsoft.com/dotnet/runtime:10.0

WORKDIR /app

COPY --from=build /.out ./

ENTRYPOINT [ "dotnet", "DistributedServers.Edge.Producer.dll" ]