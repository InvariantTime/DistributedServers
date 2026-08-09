using Confluent.Kafka;

namespace DistributedServers.Central.Consumer;

public class Worker : BackgroundService
{
    private readonly  ILogger<Worker> _logger;
    private readonly IConsumer<Ignore, string> _consumer;

    public Worker(IConsumer<Ignore, string> consumer, ILogger<Worker> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("test-topic");
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);

                _logger.LogInformation("Received message: {message}", result.Message.Value);

                await Task.Delay(100, stoppingToken);
            }
            catch (OperationCanceledException) {}
            finally
            {
                _consumer.Close();
            }
        }
    }
}