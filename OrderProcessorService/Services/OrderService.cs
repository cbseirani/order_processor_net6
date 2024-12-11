using OrderProcessorService.Clients;

namespace OrderProcessorService.Services;

public interface IOrderService
{
    Task ProcessOrders();
}

public class OrderService(IApiClient apiClient, ILogger<OrderService> logger) : IOrderService
{
    public async Task ProcessOrders()
    {
        logger.LogInformation("Processing orders...");

        // catch and contain any errors to ensure service doesn't crash
        try
        {
            var orders = await apiClient.GetOrders();
            foreach (var order in orders)
            {
                // catch and contain any errors on each order to ensure each order can be processed
                try
                {
                    if (!order.Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (order.NotificationSent)
                        continue;

                    await apiClient.SendDeliveryNotification(order.OrderId);
                    order.NotificationSent = true;
                    await apiClient.UpdateOrderStatus(order);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error processing order {order.OrderId}");
                }
            }

            logger.LogInformation("Finished processing orders.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error processing orders.");
        }
    }
}