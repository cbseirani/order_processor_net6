using OrderProcessorService.Services;

namespace OrderProcessorService;

public class Worker : BackgroundService
{
    private readonly IOrderService _orderProcessor;

    public Worker(IOrderService orderProcessor)
    {
        _orderProcessor = orderProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) 
        { 
            // Run every 60 seconds
            await _orderProcessor.ProcessOrders(); 
            await Task.Delay(60000, stoppingToken);
        }
    }
}