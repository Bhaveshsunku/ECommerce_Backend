using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Auth;

namespace ECommerce.Business.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(
        RegisterDto dto);

    Task<AuthResponseDto?> LoginAsync(
        LoginDto dto);
}