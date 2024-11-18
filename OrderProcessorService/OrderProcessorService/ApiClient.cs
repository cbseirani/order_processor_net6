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
    private readonly IConfiguration _config;
    
    public ApiClient(IConfiguration config, ILogger<ApiClient> logger)
    {
        _logger = logger;
        _config = config;
    }
    
    public async Task<JObject[]> GetOrders()
    {
        _logger.LogInformation("Getting orders");
        return await _config["ORDERS_URL"]
            .GetJsonAsync<JObject[]>();
    }

    public async Task SendDeliveryNotification(string orderId)
    {
        _logger.LogInformation("Sending delivery notification");
        var alertData = new { Message = $"Alert for delivered item: Order {orderId}" };
        await _config["ALERTS_URL"]
            .PostJsonAsync(alertData)
            .ReceiveString();
    }

    public async Task UpdateOrderStatus(JObject order)
    {
        _logger.LogInformation("Sending delivery notification");
        await _config["UPDATES_URL"]
            .PostJsonAsync(order)
            .ReceiveString();
    }
}