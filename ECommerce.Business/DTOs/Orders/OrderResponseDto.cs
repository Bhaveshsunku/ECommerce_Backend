using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Enums;

namespace ECommerce.Business.DTOs.Orders;

public class OrderResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }

    public List<OrderItemResponseDto> Items { get; set; }
        = new List<OrderItemResponseDto>();
}