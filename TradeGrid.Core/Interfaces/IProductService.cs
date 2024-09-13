using TestTask.Core.Models;

namespace TestTask.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>?> GetAllProductsAsync();
    }
}
