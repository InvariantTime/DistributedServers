using Confluent.Kafka;
using DistributedServers.Edge.Producer;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.SectionName));

builder.Services.AddSingleton(scope =>
{
    var options = scope.GetRequiredService<IOptions<KafkaOptions>>().Value;
    var logger = scope.GetRequiredService<ILogger<IProducer<Null, string>>>();
    
    var config = new ProducerConfig()
    {
        BootstrapServers = options.BootstrapServers
    };

    void ErrorHandler(IProducer<Null, string> producer,  Error error)
    {
        logger.LogError("Kafka error: {Reason}", error.Reason);
    }
    
    return new ProducerBuilder<Null, string>(config)
        .SetErrorHandler(ErrorHandler)
        .Build();
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();