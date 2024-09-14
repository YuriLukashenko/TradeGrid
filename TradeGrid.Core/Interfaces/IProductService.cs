using TradeGrid.Core.DTOs;
using TradeGrid.Core.Models;

namespace TradeGrid.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>?> GetAllProductsAsync();
        Task<IEnumerable<Product>?> GetFilteredProductsAsync(FilterDto filter);
    }
}
