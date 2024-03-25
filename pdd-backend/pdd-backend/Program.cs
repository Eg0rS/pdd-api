using System.Reflection;
using Common;
using Common.Interfaces;
using Database;
using Database.Interfaces;
using FluentMigrator.Runner;
using Kafka.Interfaces;
using Kafka.Services;
using Minio.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//существуют со старта приложения Singleton

builder.Services.AddSingleton<IConfigurationSettings, ConfigurationSettings>();
builder.Services.AddSingleton<IConnection, Connection>();
builder.Services.AddSingleton<IKafkaProducesService, KafkaProducesService>();

//создаются каждый http запрос Scoped


//создаются раз когда вызываются Transident

//существуют со старта приложения BackgroundService


builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb.AddPostgres().WithGlobalConnectionString(connectionString).ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
    .AddLogging(rb => rb.AddFluentMigratorConsole());


//minio
builder.Services.AddMinio(minioOptions =>
{
    minioOptions.AccessKey = builder.Configuration.GetSection("Minio").GetSection("AccessKey").Value;
    minioOptions.SecretKey = builder.Configuration.GetSection("Minio").GetSection("SecretKey").Value;
    minioOptions.Endpoint = builder.Configuration.GetSection("Minio").GetSection("Connection").Value;
});


var app = builder.Build();
var serviceProvider = app.Services.CreateScope().ServiceProvider;

var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();

app.Run();
