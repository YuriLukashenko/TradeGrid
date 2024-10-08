﻿using TradeGrid.Core.DTOs;
using TradeGrid.Core.Models;

namespace TradeGrid.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>?> GetAllProductsAsync();
        IEnumerable<Product>? GetFilteredProducts(IEnumerable<Product>? products, FilterDto filter);

        IEnumerable<ProductResponseDto>? Map(IEnumerable<Product>? products, string? highlight);

        Task<IEnumerable<string>> GetCommonWords(int skip, int take);
    }
}
