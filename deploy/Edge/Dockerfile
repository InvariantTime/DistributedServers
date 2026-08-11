FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet-build

WORKDIR /src

COPY ./src/Edge/Producer/ ./Edge/Producer/
COPY ./src/Shared/ ./Shared/

RUN dotnet publish "Edge/Producer/DistributedServers.Edge.Producer/DistributedServers.Edge.Producer.csproj" \
    -c Release \ 
    -r linux-musl-x64 \
    --self-contained true \
    -o /.out/producer

FROM apache/kafka:4.3.1 AS final


WORKDIR /apps

USER root

RUN apk add --no-cache \
    libgcc \
    libstdc++ \
    libssl3 \
    ca-certificates \
    icu-libs \
    icu-data-full \
    tzdata \
    krb5


COPY --from=dotnet-build .out ./
COPY deploy/Edge/entrypoint.sh ./entrypoint.sh
COPY deploy/Edge/kafka/init.sh ./scripts/kafka-init.sh


RUN chmod +x ./entrypoint.sh && \
 chmod +x ./scripts/kafka-init.sh && \
 chown appuser /apps

USER appuser

ENTRYPOINT ["./entrypoint.sh"]