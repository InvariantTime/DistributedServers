
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

LABEL project-path = "Central/Consumer/"
LABEL csproj-path="DistributedServers.Central.Consumer/DistributedServers.Central.Consumer.csproj"

WORKDIR /src

COPY "Central/Consumer/" ./Central/Consumer/

RUN dotnet publish "Central/Consumer/DistributedServers.Central.Consumer/DistributedServers.Central.Consumer.csproj" -c Release  -o /.out

FROM mcr.microsoft.com/dotnet/runtime:10.0

WORKDIR /app

COPY --from=build /.out ./

ENTRYPOINT [ "dotnet", "DistributedServers.Central.Consumer.dll" ]