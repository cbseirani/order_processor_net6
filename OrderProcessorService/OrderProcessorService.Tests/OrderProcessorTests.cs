using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace OrderProcessorService.Tests;

public class OrderProcessorTests
{
    private readonly Mock<IApiClient> _apiClient = new();
    private readonly Mock<ILogger<OrderProcessor>> _logger = new();
    
    [Fact]
    public async Task ProcessOrdersAsync_UpdatesDeliveredOrders()
    {
        // Arrange
        _apiClient.Setup(x => x.GetOrders()).ReturnsAsync(new JObject[]
        {
            new JObject { ["OrderId"] = "1", ["Status"] = "Delivered", ["deliveryNotification"] = 0 },
            new JObject { ["OrderId"] = "2", ["Status"] = "Pending", ["deliveryNotification"] = 0 }
        });

        // Act
        await CreateProcessor().ProcessOrders();

        // Assert
        _apiClient.Verify(x => x.SendDeliveryNotification(It.Is<string>(id => id == "1")), Times.Once);
        _apiClient.Verify(x => x.UpdateOrderStatus(It.Is<JObject>(order => order["OrderId"].ToString() == "1")), Times.Once);
        _apiClient.Verify(x => x.SendDeliveryNotification(It.Is<string>(id => id == "2")), Times.Never);
    }

    private OrderProcessor CreateProcessor() => new (_apiClient.Object, _logger.Object);
}