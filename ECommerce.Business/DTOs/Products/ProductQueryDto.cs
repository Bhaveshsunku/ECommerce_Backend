using System.ComponentModel.DataAnnotations;

namespace ECommerce.Business.DTOs.Products;

public class ProductQueryDto
{
    public string? Search { get; set; }

    public string? Category { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MaxPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}