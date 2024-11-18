using Newtonsoft.Json.Linq;

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
        try
        {
            var orders = await _apiClient.GetOrders();
            foreach (var order in orders)
            {
                if (!order["Status"].ToString().Equals("Delivered", StringComparison.OrdinalIgnoreCase)) 
                    continue;
                
                await _apiClient.SendDeliveryNotification(order["OrderId"].ToString());

                // instantiate notification count if null
                if (order["deliveryNotification"] is null 
                    || order["deliveryNotification"].Type is JTokenType.Null)
                {
                    order["deliveryNotification"] = 0;
                }

                order["deliveryNotification"] = (int)order["deliveryNotification"] + 1;
                
                await _apiClient.UpdateOrderStatus(order);
            }
            
            _logger.LogInformation("Finished processing orders.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error processing orders.");
        }
    }
}