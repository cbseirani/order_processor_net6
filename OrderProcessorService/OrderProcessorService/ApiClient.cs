using Flurl.Http;
using OrderProcessorService.Models;

namespace OrderProcessorService;

public interface IApiClient
{
    Task<IEnumerable<Order>> GetOrders();
    Task SendDeliveryNotification(string orderId);
    Task UpdateOrderStatus(Order order);
}

public class ApiClient : IApiClient
{
    private readonly ILogger<ApiClient> _logger;
    private readonly string _ordersUrl;
    private readonly string _updatesUrl;
    private readonly string _alertsUrl;
    
    public ApiClient(string ordersUrl, string updatesUrl, string alertsUrl, ILogger<ApiClient> logger)
    {
        _logger = logger;
        _ordersUrl = ordersUrl;
        _updatesUrl = updatesUrl;
        _alertsUrl = alertsUrl;
    }
    
    public async Task<IEnumerable<Order>> GetOrders()
    {
        try
        {
            _logger.LogInformation("Getting orders");
            return await _ordersUrl.GetJsonAsync<IEnumerable<Order>>();
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Error fetching orders from API");
            throw;
        }
    }

    public async Task SendDeliveryNotification(string orderId)
    {
        try
        {
            _logger.LogInformation("Sending delivery notification");
            var alertData = new { Message = $"Alert for delivered item: Order {orderId}" };
            await _alertsUrl.PostJsonAsync(alertData).ReceiveString();
        }
        catch(Exception e)
        {
            _logger.LogError(e, $"Error sending delivery notification for order {orderId} to alerts");
            throw;
        }
    }

    public async Task UpdateOrderStatus(Order order)
    {
        try
        {
            _logger.LogInformation("Updating order status");
            await _updatesUrl.PostJsonAsync(order).ReceiveString();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error sending order {order.OrderId} update to API");
            throw;
        }

    }
}