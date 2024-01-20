using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RabbitMqWorker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders(); // Clear default providers
        logging.AddConsole(); // Add console logger
        // Add other loggers as needed
    })
    .Build();

await host.RunAsync();