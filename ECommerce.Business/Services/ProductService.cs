using ECommerce.Business.DTOs.Products;
using ECommerce.Business.Interfaces;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.Business.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMemoryCache _cache;

    private const string ProductCacheVersionKey =
        "product_cache_version";

    public ProductService(
        IProductRepository productRepository,
        IMemoryCache cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    // ==========================================
    // Get Product Catalog
    // ==========================================

    public async Task<ProductPagedResponseDto> GetCatalogAsync(
        ProductQueryDto query)
    {
        // -------------------------------
        // Validate pagination
        // -------------------------------

        if (query.PageNumber < 1)
        {
            query.PageNumber = 1;
        }

        if (query.PageSize < 1)
        {
            query.PageSize = 10;
        }

        if (query.PageSize > 100)
        {
            query.PageSize = 100;
        }

        // -------------------------------
        // Validate price filters
        // -------------------------------

        if (query.MinPrice.HasValue &&
            query.MinPrice.Value < 0)
        {
            throw new ArgumentException(
                "Minimum price cannot be negative.");
        }

        if (query.MaxPrice.HasValue &&
            query.MaxPrice.Value < 0)
        {
            throw new ArgumentException(
                "Maximum price cannot be negative.");
        }

        if (query.MinPrice.HasValue &&
            query.MaxPrice.HasValue &&
            query.MinPrice.Value > query.MaxPrice.Value)
        {
            throw new ArgumentException(
                "Minimum price cannot be greater than maximum price.");
        }

        // -------------------------------
        // Create cache key
        // -------------------------------

        var version = GetProductCacheVersion();

        var cacheKey =
            $"products_{version}_" +
            $"{query.Search}_" +
            $"{query.Category}_" +
            $"{query.MinPrice}_" +
            $"{query.MaxPrice}_" +
            $"{query.PageNumber}_" +
            $"{query.PageSize}";

        // -------------------------------
        // Check cache
        // -------------------------------

        if (_cache.TryGetValue(
            cacheKey,
            out ProductPagedResponseDto? cachedResult))
        {
            return cachedResult!;
        }

        // -------------------------------
        // Get data from database
        // -------------------------------

        var result =
            await _productRepository.GetCatalogAsync(
                query.Search,
                query.Category,
                query.MinPrice,
                query.MaxPrice,
                query.PageNumber,
                query.PageSize);

        // -------------------------------
        // Convert entities to DTOs
        // -------------------------------

        var products = result.Products
            .Select(MapToResponseDto)
            .ToList();

        // -------------------------------
        // Calculate total pages
        // -------------------------------

        var totalPages = (int)Math.Ceiling(
            result.TotalCount /
            (double)query.PageSize);

        // -------------------------------
        // Create response
        // -------------------------------

        var response = new ProductPagedResponseDto
        {
            Products = products,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = totalPages
        };

        // -------------------------------
        // Store in cache
        // -------------------------------

        _cache.Set(
            cacheKey,
            response,
            TimeSpan.FromMinutes(5));

        return response;
    }

    // ==========================================
    // Get All Products
    // ==========================================

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var products =
            await _productRepository.GetAllAsync();

        return products
            .Select(MapToResponseDto)
            .ToList();
    }

    // ==========================================
    // Get Product By ID
    // ==========================================

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product =
            await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return null;
        }

        return MapToResponseDto(product);
    }

    // ==========================================
    // Create Product
    // ==========================================

    public async Task<ProductResponseDto> CreateAsync(
        CreateProductDto dto)
    {
        // Validate price
        if (dto.Price <= 0)
        {
            throw new ArgumentException(
                "Product price must be greater than zero.");
        }

        // Create entity
        var product = new Product
        {
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Price = dto.Price,
            Category = dto.Category.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Save product
        var createdProduct =
            await _productRepository.AddAsync(product);

        // Invalidate catalog cache
        InvalidateProductCache();

        return MapToResponseDto(createdProduct);
    }

    // ==========================================
    // Update Product
    // ==========================================

    public async Task<bool> UpdateAsync(
        int id,
        UpdateProductDto dto)
    {
        // Find product
        var product =
            await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        // Validate price
        if (dto.Price <= 0)
        {
            throw new ArgumentException(
                "Product price must be greater than zero.");
        }

        // Update properties
        product.Name = dto.Name.Trim();
        product.Description = dto.Description.Trim();
        product.Price = dto.Price;
        product.Category = dto.Category.Trim();
        product.IsActive = dto.IsActive;

        // Save changes
        await _productRepository.UpdateAsync(product);

        // Invalidate catalog cache
        InvalidateProductCache();

        return true;
    }

    // ==========================================
    // Delete Product
    // ==========================================

    public async Task<bool> DeleteAsync(int id)
    {
        // Find product
        var product =
            await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        // Delete product
        await _productRepository.DeleteAsync(product);

        // Invalidate catalog cache
        InvalidateProductCache();

        return true;
    }

    // ==========================================
    // Get Cache Version
    // ==========================================

    private string GetProductCacheVersion()
    {
        var version =
            _cache.Get<string>(
                ProductCacheVersionKey);

        if (string.IsNullOrEmpty(version))
        {
            version = Guid.NewGuid().ToString();

            _cache.Set(
                ProductCacheVersionKey,
                version);
        }

        return version;
    }

    // ==========================================
    // Invalidate Product Cache
    // ==========================================

    private void InvalidateProductCache()
    {
        _cache.Set(
            ProductCacheVersionKey,
            Guid.NewGuid().ToString());
    }

    // ==========================================
    // Map Entity → DTO
    // ==========================================

    private static ProductResponseDto MapToResponseDto(
        Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}