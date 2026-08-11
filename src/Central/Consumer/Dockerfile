
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build


WORKDIR /src

COPY ./Central/Consumer/ ./Central/Consumer/
COPY ./Shared/ ./Shared/

RUN dotnet publish "Central/Consumer/DistributedServers.Central.Consumer/DistributedServers.Central.Consumer.csproj" -c Release  -o /.out

FROM mcr.microsoft.com/dotnet/runtime:10.0

WORKDIR /app

COPY --from=build /.out ./

ENTRYPOINT [ "dotnet", "DistributedServers.Central.Consumer.dll" ]