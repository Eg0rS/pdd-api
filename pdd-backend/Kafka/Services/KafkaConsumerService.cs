using Common.Interfaces;
using Confluent.Kafka;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kafka.Services;

public class KafkaConsumerService : BackgroundService
{
    
    private readonly IConsumer<Null, string> consumer;
    private readonly ILogger<KafkaConsumerService> logger;
    private readonly string topic;
    
    public KafkaConsumerService(IConfigurationSettings settings, ILogger<KafkaConsumerService> logger)
    {
        this.logger = logger;
        topic = settings.KafkaSettings.KafkaTopicConsumer;
        consumer = new ConsumerBuilder<Null, string>(new ConsumerConfig
        {
            BootstrapServers = settings.KafkaSettings.KafkaConnection,
            AutoOffsetReset = AutoOffsetReset.Earliest
        }).SetLogHandler((_, logMessage) =>
        {
            if (logMessage.Level != SyslogLevel.Info && logMessage.Level != SyslogLevel.Debug)
            {
                this.logger.LogError($"Kafka consumer error. {logMessage.Message}");
            }
        }).Build();
        consumer.Subscribe(topic);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        var i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);

            logger.LogInformation(consumeResult.Message.Key + " - " + consumeResult.Message.Value);

            if (i++ % 1000 == 0)
            {
                consumer.Commit();
            }
        }
    }

    public override void Dispose()
    {
        consumer.Dispose();
        base.Dispose();
    }
    
    
}
