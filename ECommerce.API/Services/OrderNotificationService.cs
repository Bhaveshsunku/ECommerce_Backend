using ECommerce.API.Hubs;
using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.Services;

public class OrderNotificationService
    : IOrderNotificationService
{
    private readonly IHubContext<OrderNotificationHub> _hubContext;

    public OrderNotificationService(
        IHubContext<OrderNotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyOrderCreatedAsync(
        int userId,
        int orderId,
        decimal totalAmount)
    {
        await _hubContext.Clients
            .User(userId.ToString())
            .SendAsync(
                "OrderCreated",
                new
                {
                    OrderId = orderId,
                    TotalAmount = totalAmount,
                    Message =
                        $"Order #{orderId} has been created successfully."
                });
    }

    public async Task NotifyOrderCancelledAsync(
        int userId,
        int orderId)
    {
        await _hubContext.Clients
            .User(userId.ToString())
            .SendAsync(
                "OrderCancelled",
                new
                {
                    OrderId = orderId,
                    Message =
                        $"Order #{orderId} has been cancelled."
                });
    }
}