using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Business.DTOs.Orders;

public class CreateOrderDto
{
    [Required]
    [MinLength(1)]
    public List<CreateOrderItemDto> Items { get; set; }
        = new List<CreateOrderItemDto>();
}