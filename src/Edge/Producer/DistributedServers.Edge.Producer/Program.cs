using Confluent.Kafka;
using DistributedServers.Edge.Producer;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.SectionName));

builder.Services.AddSingleton(scope =>
{
    var options = scope.GetRequiredService<IOptions<KafkaOptions>>().Value;
    var logger = scope.GetRequiredService<ILogger<IProducer<string, string>>>();
    
    var config = new ProducerConfig()
    {
        BootstrapServers = options.BootstrapServers
    };

    void ErrorHandler(IProducer<string, string> producer,  Error error)
    {
        logger.LogError("Kafka error: {Reason}", error.Reason);
    }
    
    return new ProducerBuilder<string, string>(config)
        .SetErrorHandler(ErrorHandler)
        .Build();
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();