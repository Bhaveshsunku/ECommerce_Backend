using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities;

public class Inventory
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Product Product { get; set; } = null!;
}
