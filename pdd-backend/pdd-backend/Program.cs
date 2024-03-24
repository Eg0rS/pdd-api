using System.Reflection;
using FluentMigrator.Runner;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//существуют со старта приложения Singleton



//создаются каждый http запрос Scoped


//создаются раз когда вызываются Transident


builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb.AddPostgres().WithGlobalConnectionString(connectionString).ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
    .AddLogging(rb => rb.AddFluentMigratorConsole());


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
