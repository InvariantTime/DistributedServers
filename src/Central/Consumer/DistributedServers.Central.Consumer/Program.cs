using Confluent.Kafka;
using DistributedServers.Central.Consumer;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.SectionName));

builder.Services.AddSingleton(scope =>
{
    var options = scope.GetRequiredService<IOptions<KafkaOptions>>().Value;

    var config = new ConsumerConfig()
    {
        BootstrapServers = options.BootstrapServers,
        GroupId = options.ConsumerGroupId,
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    var logger = scope.GetRequiredService<ILogger<IConsumer<string, string>>>();
    
    void ErrorHandler(IConsumer<string, string> consumer, Error error)
    {
        logger.LogError("Kafka error: {reason}", error.Reason);
    }

    return new ConsumerBuilder<string, string>(config)
        .SetErrorHandler(ErrorHandler)
        .Build();
});

builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();