using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Domain.Entities;

namespace ECommerce.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByEmailAsync(string email);

    Task<User> AddAsync(User user);
}