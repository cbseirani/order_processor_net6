using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace OrderProcessorService.Tests;

public class ApiClientTests
{
    private readonly Mock<ILogger<ApiClient>> _logger = new();

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
    
    private ApiClient CreateClient() => new (_logger.Object);
}


