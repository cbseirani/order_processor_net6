namespace OrderProcessorService;

public interface IOrderProcessor
{
    Task ProcessOrders();
}

public class OrderProcessor : IOrderProcessor
{
    private readonly IApiClient _apiClient;
    private readonly ILogger<OrderProcessor> _logger;

    public OrderProcessor(IApiClient apiClient, ILogger<OrderProcessor> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task ProcessOrders()
    {
        _logger.LogInformation("Processing orders...");
        
        // catch and contain any errors to ensure service doesn't crash
        try
        {
            var orders = await _apiClient.GetOrders();
            foreach (var order in orders)
            {
                // catch and contain any errors on each order to ensure each order can be processed
                try
                {
                    if (!order.Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
                        continue;

                    await _apiClient.SendDeliveryNotification(order.OrderId);
                    order.NotificationCount += 1;

                    await _apiClient.UpdateOrderStatus(order);
                }
                catch (Exception e)
                {
                    _logger.LogError(e,$"Error processing order {order.OrderId}");
                }
            }
            
            _logger.LogInformation("Finished processing orders.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error processing orders.");
        }
    }
}