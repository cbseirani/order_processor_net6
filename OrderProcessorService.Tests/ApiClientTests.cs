using Flurl.Http.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessorService.Clients;
using Xunit;

namespace OrderProcessorService.Tests;

public class ApiClientTests
{
    private readonly Mock<IConfiguration> _config = new();
    private readonly Mock<ILogger<ApiClient>> _logger = new();
    
    public ApiClientTests() 
    { 
        // Set up mock configuration values
        _config.SetupGet(x => x["ORDERS_URL"])
            .Returns("https://orders-api.com/orders");
        
        _config.SetupGet(x => x["ALERTS_URL"])
            .Returns("https://alert-api.com/alerts");
        
        _config.SetupGet(x => x["UPDATES_URL"])
            .Returns("https://update-api.com/update"); 
    }
    
    [Fact]
    public async Task SendDeliveryNotificationAsync_SendsNotification()
    {
        using var httpTest = new HttpTest();
        
        // Arrange
        httpTest.RespondWith("");

        // Act
        await CreateClient().SendDeliveryNotification("1");

        // Assert
        httpTest.ShouldHaveCalled("https://alert-api.com/alerts")
            .WithVerb(HttpMethod.Post)
            .WithRequestBody("{\"Message\":\"Alert for delivered item: Order 1\"}")
            .Times(1);
    }
    
    private ApiClient CreateClient() => new (
        _config.Object["ORDERS_URL"], 
        _config.Object["UPDATES_URL"], 
        _config.Object["ALERTS_URL"], 
        _logger.Object);
}


