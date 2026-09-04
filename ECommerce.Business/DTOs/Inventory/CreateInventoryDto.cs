using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Business.DTOs.Inventory;

public class CreateInventoryDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}