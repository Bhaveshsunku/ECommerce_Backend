using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.Services;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using Moq;
using Microsoft.Extensions.Caching.Memory; // added

namespace ECommerce.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ProductExists_ReturnsProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Test laptop",
            Price = 50000,
            Category = "Electronics",
            IsActive = true
        };

        var repository = new Mock<IProductRepository>();

        repository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var service =
            new ProductService(repository.Object, memoryCache); // pass cache

        // Act
        var result =
            await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal(50000, result.Price);
    }
}

