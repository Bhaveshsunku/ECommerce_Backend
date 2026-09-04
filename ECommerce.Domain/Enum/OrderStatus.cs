using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Enums;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}