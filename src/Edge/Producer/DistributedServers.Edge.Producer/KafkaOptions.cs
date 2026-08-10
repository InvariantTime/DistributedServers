using Confluent.Kafka;

namespace DistributedServers.Edge.Producer;

public sealed class KafkaOptions//TODO: move to shared project
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; init; } = string.Empty;
}