namespace Common.Interfaces;

public interface IConfigurationSettings
{
    public string DbConnection { get; }
    public KafkaSettings KafkaSettings { get; }
}
