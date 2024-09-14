using TradeGrid.Core.DTOs;
using TradeGrid.Core.Interfaces;
using TradeGrid.Core.Models;

namespace TradeGrid.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private const string _sourceUrl = "https://pastebin.com/raw/JucRNpWs";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>?> GetAllProductsAsync()
        {
            var products = Enumerable.Empty<Product>();

            //Enumerable.Empty<Product>() or new List<Product>() ?
            //the first one: The collection is immutable, better for performance
            //the second one: if collection is mutable and you plan to use .Add(), .Remove() etc.

            try
            {
                var productDto = await _httpClient.GetFromJsonAsync<ProductDto>(_sourceUrl);

                products = productDto?.Products ?? Enumerable.Empty<Product>();

                //another approach is like that: 

                //var response = await _httpClient.GetAsync(_sourceUrl);
                //response.EnsureSuccessStatusCode();
                //var data = await response.Content.ReadAsStringAsync();
                //products = JsonSerializer.Deserialize<IEnumerable<Product>>(data);

                //it is more code, but it could be useful if we need custom deserialization,
                //non-json response of request or adding custom handling.
                //Otherwise, it's better to use GetFromJsonAsync for cleaner, simpler, and more efficient code. 

                //todo use logging
                Console.WriteLine("Received products:");
                Console.WriteLine(products);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }

            return products;
        }

        public async Task<IEnumerable<Product>?> GetFilteredProductsAsync(FilterDto filter)
        {
            var filteredProducts = (await GetAllProductsAsync())?.AsQueryable(); //better to use queryable, because it creates query and only then apply it into source

            if (filter.MinPrice.HasValue)
            {
                filteredProducts = filteredProducts?.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                filteredProducts = filteredProducts?.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            //one way is to use foreach for filter.Sizes
            //foreach (var size in filter.Sizes ?? Enumerable.Empty<string>())
            //{
            //    filteredProducts = filteredProducts?.Where(x => x.Sizes.Any(s => s == size));
            //}
            //the second - use Intersect, more readable

            if (filter.Sizes?.Any() ?? false)
            {
                filteredProducts = filteredProducts?.Where(x => x.Sizes.Intersect(filter.Sizes).Any());
            }

            var count = filteredProducts?.Count();

            return filteredProducts;
        }
    }
}
