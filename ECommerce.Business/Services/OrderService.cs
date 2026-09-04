using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Orders;
using ECommerce.Business.Interfaces;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Business.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
    }

    public async Task<OrderResponseDto> CreateAsync(
        int userId,
        CreateOrderDto dto)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "Invalid user ID.");
        }

        if (dto.Items == null || dto.Items.Count == 0)
        {
            throw new ArgumentException(
                "Order must contain at least one item.");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalAmount = 0
        };

        decimal totalAmount = 0;

        foreach (var itemDto in dto.Items)
        {
            if (itemDto.Quantity <= 0)
            {
                throw new ArgumentException(
                    "Quantity must be greater than zero.");
            }

            var product =
                await _productRepository.GetByIdAsync(
                    itemDto.ProductId);

            if (product == null)
            {
                throw new ArgumentException(
                    $"Product {itemDto.ProductId} does not exist.");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException(
                    $"Product {product.Name} is not active.");
            }

            var inventory =
                await _inventoryRepository.GetByProductIdAsync(
                    itemDto.ProductId);

            if (inventory == null)
            {
                throw new InvalidOperationException(
                    $"Inventory not found for product {product.Name}.");
            }

            if (inventory.Quantity < itemDto.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for product {product.Name}.");
            }

            inventory.Quantity -= itemDto.Quantity;
            inventory.UpdatedAt = DateTime.UtcNow;

            await _inventoryRepository.UpdateAsync(inventory);

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };

            order.OrderItems.Add(orderItem);

            totalAmount +=
                product.Price * itemDto.Quantity;
        }

        order.TotalAmount = totalAmount;

        var createdOrder =
            await _orderRepository.AddAsync(order);

        return MapToResponseDto(createdOrder);
    }

    public async Task<List<OrderResponseDto>> GetMyOrdersAsync(
        int userId)
    {
        var orders =
            await _orderRepository.GetByUserIdAsync(userId);

        return orders
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<OrderResponseDto?> GetByIdAsync(
        int orderId,
        int userId)
    {
        var order =
            await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return null;
        }

        if (order.UserId != userId)
        {
            return null;
        }

        return MapToResponseDto(order);
    }

    public async Task<bool> CancelAsync(
        int orderId,
        int userId)
    {
        var order =
            await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return false;
        }

        if (order.UserId != userId)
        {
            return false;
        }

        if (order.Status == OrderStatus.Cancelled)
        {
            return false;
        }

        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending orders can be cancelled.");
        }

        order.Status = OrderStatus.Cancelled;

        await _orderRepository.UpdateAsync(order);

        return true;
    }

    private static OrderResponseDto MapToResponseDto(
        Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,

            Items = order.OrderItems
                .Select(item => new OrderItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    SubTotal =
                        item.UnitPrice * item.Quantity
                })
                .ToList()
        };
    }
}