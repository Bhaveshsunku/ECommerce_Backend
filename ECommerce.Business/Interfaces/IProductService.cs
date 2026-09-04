using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Products;

namespace ECommerce.Business.Interfaces;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllAsync();

    Task<ProductResponseDto?> GetByIdAsync(int id);

    Task<ProductResponseDto> CreateAsync(
        CreateProductDto dto);

    Task<bool> UpdateAsync(
        int id,
        UpdateProductDto dto);

    Task<bool> DeleteAsync(int id);

    Task<ProductPagedResponseDto> GetCatalogAsync(
        ProductQueryDto query);
}