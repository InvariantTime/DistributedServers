using Confluent.Kafka;

namespace DistributedServers.Edge.Producer;

public class Worker : BackgroundService
{
    private static readonly string[] _messages = ["a-message", "b-message", "c-message", "d-message", "e-message"];
    
    private readonly IProducer<Null, string> _producer;
    private readonly  ILogger<Worker> _logger;

    public Worker(IProducer<Null, string> producer, ILogger<Worker> logger)
    {
        _producer = producer;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            var result = await _producer.ProduceAsync("test-events", new Message<Null, string>()
            {
                Value = _messages[Random.Shared.Next(0, _messages.Length)]
            }, stoppingToken);
            
            _logger.LogInformation("Delivered '{Value}' to '{PartitionOffset}'", result.Value, result.TopicPartitionOffset);
            
            await Task.Delay(500, stoppingToken);
        }
    }
}