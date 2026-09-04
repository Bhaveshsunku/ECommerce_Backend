using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace ECommerce.Business.DTOs.Inventory;

public class UpdateInventoryDto
{
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}