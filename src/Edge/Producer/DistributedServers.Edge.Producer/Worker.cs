using System.Text.Json;
using Confluent.Kafka;
using DistributedServers.Contracts.Events;

namespace DistributedServers.Edge.Producer;

public class Worker : BackgroundService
{
    private static readonly string[] _types = Enumerable.Range('a', 'z').Select(x => x.ToString()).ToArray();
    private static readonly string[] _keys =  Enumerable.Range('a', 'z').Select(x => $"key-{x.ToString()}").ToArray();
    
    private readonly IProducer<string, string> _producer;
    private readonly  ILogger<Worker> _logger;

    public Worker(IProducer<string, string> producer, ILogger<Worker> logger)
    {
        _producer = producer;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            var @event = new GenericEvent
            {
                EventId =  Guid.NewGuid(),
                EventType = _types[Random.Shared.Next(_types.Length)],
                Key = _keys[Random.Shared.Next(_keys.Length)],
                Value =  Random.Shared.Next(0, int.MaxValue)
            };
            
            var result = await _producer.ProduceAsync("test-events", new Message<string, string>()
            {
                Value = JsonSerializer.Serialize(@event),
                Key = @event.Key
            }, stoppingToken);
            
            _logger.LogInformation("Delivered '{Value}' to '{PartitionOffset}'", result.Value, result.TopicPartitionOffset);
            
            await Task.Delay(500, stoppingToken);
        }
    }
}