using Microsoft.AspNetCore.Http.Metadata;
using System.Text.Json;
using TestTask.Core.DTOs;
using TestTask.Core.Interfaces;
using TestTask.Core.Models;

namespace TestTask.Services
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
    }
}
