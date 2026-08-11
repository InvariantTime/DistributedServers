using System.Text.Json;
using Confluent.Kafka;
using DistributedServers.Contracts.Events;

namespace DistributedServers.Central.Consumer;

public class Worker : BackgroundService
{
    private readonly  ILogger<Worker> _logger;
    private readonly IConsumer<string, string> _consumer;

    public Worker(IConsumer<string, string> consumer, ILogger<Worker> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("test-events");

        try
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                var result = _consumer.Consume(stoppingToken);

                var @event = JsonSerializer.Deserialize<GenericEvent>(result.Message.Value);
                
                if (@event == null)
                    continue;//TODO: handle invalid messages
                
                _logger.LogInformation("Received event: Id: {Id}, Type: {Type}, Key: {Key}, Value: {Value}", 
                    @event.EventId, @event.EventType, @event.Key, @event.Value);
            }
        }
        catch (OperationCanceledException) {}
        
        return Task.CompletedTask;
    }
}