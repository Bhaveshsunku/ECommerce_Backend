using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Orders;
using ECommerce.Business.Services;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using Moq;

namespace ECommerce.Tests;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateAsync_InsufficientStock_ThrowsException()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 50000,
            IsActive = true
        };

        var inventory = new Inventory
        {
            ProductId = 1,
            Quantity = 2
        };

        var productRepository =
            new Mock<IProductRepository>();

        productRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        var inventoryRepository =
            new Mock<IInventoryRepository>();

        inventoryRepository
            .Setup(x => x.GetByProductIdAsync(1))
            .ReturnsAsync(inventory);

        var orderRepository =
            new Mock<IOrderRepository>();

        var service = new OrderService(
            orderRepository.Object,
            productRepository.Object,
            inventoryRepository.Object);

        var dto = new CreateOrderDto
        {
            Items = new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto
                {
                    ProductId = 1,
                    Quantity = 5
                }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(1, dto));
    }
}