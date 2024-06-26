﻿using Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Common;

public class ConfigurationSettings : IConfigurationSettings
{
    private readonly IConfiguration configuration;

    public ConfigurationSettings(IConfiguration configuration)

    {
        this.configuration = configuration;
    }

    public string DbConnection => configuration.GetSection("ConnectionStrings").GetSection("DatabaseConnection").Value;

    public KafkaSettings KafkaSettings => new KafkaSettings
    {
        KafkaConnection = configuration.GetSection("Kafka").GetSection("Connection").Value,
        KafkaTopicProducer = configuration.GetSection("Kafka").GetSection("TopicProducer").Value,
        KafkaTopicConsumer = configuration.GetSection("Kafka").GetSection("TopicConsumer").Value
    };
}
