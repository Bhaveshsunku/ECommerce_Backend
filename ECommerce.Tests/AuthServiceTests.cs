using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Auth;
using ECommerce.Business.Services;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ECommerce.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PasswordHash = "hashed-password",
            Role = "Customer"
        };

        var userRepository =
            new Mock<IUserRepository>();

        userRepository
            .Setup(x => x.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        var passwordHasher =
            new Mock<IPasswordHasher<User>>();

        passwordHasher
            .Setup(x => x.VerifyHashedPassword(
                user,
                user.PasswordHash,
                "WrongPassword"))
            .Returns(PasswordVerificationResult.Failed);

        var configuration =
            new Mock<IConfiguration>();

        var service = new AuthService(
            userRepository.Object,
            passwordHasher.Object,
            configuration.Object);

        var dto = new LoginDto
        {
            Email = "test@example.com",
            Password = "WrongPassword"
        };

        // Act
        var result =
            await service.LoginAsync(dto);

        // Assert
        Assert.Null(result);
    }
}