using Flurl.Http;
using OrderProcessorService.Models;

namespace OrderProcessorService.Clients;

public interface IApiClient
{
    Task<IEnumerable<Order>> GetOrders();
    Task SendDeliveryNotification(string orderId);
    Task UpdateOrderStatus(Order order);
}

public class ApiClient(string ordersUrl, string updatesUrl, string alertsUrl, ILogger<ApiClient> logger)
    : IApiClient
{
    public async Task<IEnumerable<Order>> GetOrders()
    {
        try
        {
            logger.LogInformation("Getting orders");
            return await ordersUrl.GetJsonAsync<IEnumerable<Order>>();
        }
        catch(Exception e)
        {
            logger.LogError(e, "Error fetching orders from API");
            throw;
        }
    }

    public async Task SendDeliveryNotification(string orderId)
    {
        try
        {
            logger.LogInformation("Sending delivery notification");
            var alertData = new { Message = $"Alert for delivered item: Order {orderId}" };
            await alertsUrl.PostJsonAsync(alertData).ReceiveString();
        }
        catch(Exception e)
        {
            logger.LogError(e, $"Error sending delivery notification for order {orderId} to alerts");
            throw;
        }
    }

    public async Task UpdateOrderStatus(Order order)
    {
        try
        {
            logger.LogInformation("Updating order status");
            await updatesUrl.PostJsonAsync(order).ReceiveString();
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error sending order {order.OrderId} update to API");
            throw;
        }

    }
}