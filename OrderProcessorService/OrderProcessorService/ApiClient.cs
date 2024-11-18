using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace OrderProcessorService;

public interface IApiClient
{
    Task<JObject[]> GetOrders();
    Task SendDeliveryNotification(string orderId);
    Task UpdateOrderStatus(JObject order);
}

public class ApiClient : IApiClient
{
    private readonly ILogger<ApiClient> _logger;
    
    public ApiClient(ILogger<ApiClient> logger)
    {
        _logger = logger;
    }
    
    public async Task<JObject[]> GetOrders()
    {
        _logger.LogInformation("Getting orders");
        return await "https://orders-api.com/orders"
            .GetJsonAsync<JObject[]>();
    }

    public async Task SendDeliveryNotification(string orderId)
    {
        _logger.LogInformation("Sending delivery notification");
        var alertData = new { Message = $"Alert for delivered item: Order {orderId}" };
        await "https://alert-api.com/alerts"
            .PostJsonAsync(alertData)
            .ReceiveString();
    }

    public async Task UpdateOrderStatus(JObject order)
    {
        _logger.LogInformation("Sending delivery notification");
        await "https://update-api.com/update"
            .PostJsonAsync(order)
            .ReceiveString();
    }
}