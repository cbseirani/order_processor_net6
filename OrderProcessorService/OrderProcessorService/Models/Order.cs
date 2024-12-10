namespace OrderProcessorService.Models;

public class Order
{
    public string OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int NotificationCount { get; set; }
}