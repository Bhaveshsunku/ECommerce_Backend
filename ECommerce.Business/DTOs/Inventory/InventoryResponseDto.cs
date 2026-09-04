using System;
using System.Collections.Generic;
using System.Text;
namespace ECommerce.Business.DTOs.Inventory;

public class InventoryResponseDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime UpdatedAt { get; set; }
}