using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.Business.BackgroundServices;

public class OrderExpirationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OrderExpirationService> _logger;

    public OrderExpirationService(
        IServiceScopeFactory scopeFactory,
        ILogger<OrderExpirationService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ExpireOldOrdersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while processing expired orders.");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(5),
                stoppingToken);
        }
    }

    private async Task ExpireOldOrdersAsync()
    {
        using var scope =
            _scopeFactory.CreateScope();

        var orderRepository =
            scope.ServiceProvider
                .GetRequiredService<IOrderRepository>();

        var cutoff =
            DateTime.UtcNow.AddMinutes(-30);

        var oldOrders =
            await orderRepository
                .GetPendingOrdersOlderThanAsync(cutoff);

        foreach (var order in oldOrders)
        {
            order.Status =
                OrderStatus.Cancelled;

            await orderRepository.UpdateAsync(order);

            _logger.LogInformation(
                "Order {OrderId} expired and was cancelled.",
                order.Id);
        }
    }
}