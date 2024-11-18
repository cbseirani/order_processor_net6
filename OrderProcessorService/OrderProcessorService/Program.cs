using OrderProcessorService;
using Serilog;
using Serilog.Events;

// get and set min log level from env vars
var logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "Information"; 
var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), logLevel, true);

// create Serilog logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(logEventLevel)
    .WriteTo.Console()
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(Log.Logger);
        services.AddHostedService<Worker>();
        services.AddTransient<IOrderProcessor, OrderProcessor>();
        services.AddTransient<IApiClient, ApiClient>();
    })
    .Build();

await host.RunAsync();