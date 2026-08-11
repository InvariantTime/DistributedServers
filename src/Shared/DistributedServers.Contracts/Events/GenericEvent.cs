namespace DistributedServers.Contracts.Events;

public sealed record GenericEvent
{
    public required Guid EventId { get; init; }
    
    public required string EventType { get; init; }
    
    public required int Value { get; init; }
    
    public string? Key { get; init; }
}