#!/bin/bash

set -e

/etc/kafka/docker/run & 
KAFKA_PID=$!

until /opt/kafka/bin/kafka-topics.sh \
    --bootstrap-server localhost:9092 \
    --list > /dev/null 2>&1

do
    if ! kill -0  "$KAFKA_PID" 2>/dev/null; then
        echo "Kafka failed"
        wait "$KAFKA_PID"
        exit $?
    fi

    sleep 1
done

echo "kafka is ready"

/apps/scripts/kafka-init.sh

echo "Starting Producer..."


Kafka__BootstrapServers=localhost:9092 \
/apps/producer/DistributedServers.Edge.Producer &
PRODUCER_PID=$!

echo "Producer started"

wait -n "$KAFKA_PID" "$PRODUCER_PID"