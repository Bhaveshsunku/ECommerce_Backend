using System.Text;

using ECommerce.API.Hubs;
using ECommerce.API.Middleware;
using ECommerce.API.Services;

using ECommerce.Business.BackgroundServices;
using ECommerce.Business.Interfaces;
using ECommerce.Business.Services;

using ECommerce.DataAccess.Context;
using ECommerce.DataAccess.Repositories.Implementations;
using ECommerce.DataAccess.Repositories.Interfaces;

using ECommerce.Domain.Entities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// Controllers
// ==========================================

builder.Services.AddControllers();

// ==========================================
// SignalR
// ==========================================

builder.Services.AddSignalR();

// ==========================================
// Memory Cache
// ==========================================

builder.Services.AddMemoryCache();

// ==========================================
// JWT Authentication
// ==========================================

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

// ==========================================
// Authorization
// ==========================================

builder.Services.AddAuthorization();

// ==========================================
// Swagger
// ==========================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// Entity Framework Core
// ==========================================

builder.Services.AddDbContext<EcommerceDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection")));

// ==========================================
// Repositories
// ==========================================

builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    IProductRepository,
    ProductRepository>();

builder.Services.AddScoped<
    IInventoryRepository,
    InventoryRepository>();

builder.Services.AddScoped<
    IOrderRepository,
    OrderRepository>();

builder.Services.AddScoped<
    IPaymentRepository,
    PaymentRepository>();

// ==========================================
// Business Services
// ==========================================

builder.Services.AddScoped<
    IProductService,
    ProductService>();

builder.Services.AddScoped<
    IInventoryService,
    InventoryService>();

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IOrderService,
    OrderService>();

builder.Services.AddScoped<
    IPaymentService,
    PaymentService>();

// ==========================================
// Order Notification Service
// ==========================================

builder.Services.AddScoped<
    IOrderNotificationService,
    OrderNotificationService>();

// ==========================================
// Password Hasher
// ==========================================

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();

// ==========================================
// Background Services
// ==========================================

// Automatically checks and cancels
// old pending orders.
builder.Services.AddHostedService<OrderExpirationService>();

// ==========================================
// Build Application
// ==========================================

var app = builder.Build();

// ==========================================
// Global Exception Handling
// ==========================================

app.UseMiddleware<ExceptionHandlingMiddleware>();

// ==========================================
// Swagger
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==========================================
// Authentication
// ==========================================

app.UseAuthentication();

// ==========================================
// Authorization
// ==========================================

app.UseAuthorization();

// ==========================================
// Controllers
// ==========================================

app.MapControllers();

// ==========================================
// SignalR Hub
// ==========================================

app.MapHub<OrderNotificationHub>(
    "/hubs/orders");

// ==========================================
// Run Application
// ==========================================

app.Run();