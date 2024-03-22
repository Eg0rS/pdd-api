using Common.Interfaces;
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
}
