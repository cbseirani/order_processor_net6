using OrderProcessorService;
using OrderProcessorService.Services;
using Serilog;
using Serilog.Events;

var configBuilder = new ConfigurationBuilder() 
    .AddEnvironmentVariables(); 

var configuration = configBuilder.Build();

// get and set min log level from env vars
var logLevel = configuration["LOG_LEVEL"] ?? "Information"; 
var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), logLevel, true);
var logName = configuration["LOG_NAME"] ?? "OrderProcessorLog.log";

// create Serilog logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(logEventLevel)
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, logName), 
        rollingInterval: RollingInterval.Month,
        buffered: false)
    .WriteTo.Console()
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
        services.AddHostedService<Worker>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IApiClient, ApiClient>(provider => 
            new ApiClient(
                configuration["ORDERS_URL"],
                configuration["ALERTS_URL"], 
                configuration["UPDATES_URL"],
                provider.GetRequiredService<ILogger<ApiClient>>())); 
    })
    .Build();

await host.RunAsync();