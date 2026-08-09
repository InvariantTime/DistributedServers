using Confluent.Kafka;

namespace DistributedServers.Central.Consumer;

public sealed class KafkaOptions
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; init; } = string.Empty;
    
    public string ConsumerGroupId { get; init; } = string.Empty;
    
    public AutoOffsetReset AutoOffsetReset { get; init; } = AutoOffsetReset.Earliest;
}