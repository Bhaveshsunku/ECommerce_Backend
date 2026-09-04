using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Business.DTOs.Products;

public class ProductPagedResponseDto
{
    public List<ProductResponseDto> Products { get; set; }
        = new List<ProductResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}