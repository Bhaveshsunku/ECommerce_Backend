using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Payments;
using ECommerce.Business.Services;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Moq;

namespace ECommerce.Tests;

public class PaymentServiceTests
{
    [Fact]
    public async Task CheckoutAsync_PendingOrder_CreatesSuccessfulPayment()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            UserId = 1,
            TotalAmount = 1000,
            Status = OrderStatus.Pending
        };

        var orderRepository =
            new Mock<IOrderRepository>();

        orderRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(order);

        var paymentRepository =
            new Mock<IPaymentRepository>();

        paymentRepository
            .Setup(x => x.GetByOrderIdAsync(1))
            .ReturnsAsync((Payment?)null);

        paymentRepository
            .Setup(x => x.AddAsync(It.IsAny<Payment>()))
            .ReturnsAsync((Payment payment) =>
            {
                payment.Id = 1;
                return payment;
            });

        var service = new PaymentService(
            orderRepository.Object,
            paymentRepository.Object);

        var dto = new CheckoutDto
        {
            PaymentMethod = "MockCard"
        };

        // Act
        var result =
            await service.CheckoutAsync(
                1,
                1,
                dto);

        // Assert
        Assert.Equal(
            PaymentStatus.Successful,
            result.PaymentStatus);

        Assert.Equal(1000, result.Amount);

        Assert.Equal(
            OrderStatus.Confirmed,
            order.Status);
    }
}