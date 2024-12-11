namespace OrderProcessorService.Services;

public interface IOrderService
{
    Task ProcessOrders();
}

public class OrderService : IOrderService
{
    private readonly IApiClient _apiClient;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IApiClient apiClient, ILogger<OrderService> logger)
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

                    if (order.NotificationSent)
                        continue;

                    await _apiClient.SendDeliveryNotification(order.OrderId);
                    order.NotificationSent = true;
                    await _apiClient.UpdateOrderStatus(order);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error processing order {order.OrderId}");
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