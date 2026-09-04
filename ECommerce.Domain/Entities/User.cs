using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Customer";

    public DateTime CreatedAt { get; set; }

    public ICollection<Order> Orders { get; set; }
        = new List<Order>();
}