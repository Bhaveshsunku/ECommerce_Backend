using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Orders;

namespace ECommerce.Business.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(
        int userId,
        CreateOrderDto dto);

    Task<List<OrderResponseDto>> GetMyOrdersAsync(
        int userId);

    Task<OrderResponseDto?> GetByIdAsync(
        int orderId,
        int userId);

    Task<bool> CancelAsync(
        int orderId,
        int userId);
}