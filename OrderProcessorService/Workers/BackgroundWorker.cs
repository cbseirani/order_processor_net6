using OrderProcessorService.Services;

namespace OrderProcessorService.Workers;

public class BackgroundWorker(IOrderService orderProcessor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) 
        {
            await orderProcessor.ProcessOrders(); 
            await Task.Delay(60000, stoppingToken);// Run every 60 seconds
        }
    }
}