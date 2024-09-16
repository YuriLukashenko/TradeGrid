using Newtonsoft.Json;
using TradeGrid.Core;
using TradeGrid.Core.DTOs;
using TradeGrid.Core.Interfaces;
using TradeGrid.Core.Models;

namespace TradeGrid.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;
        private const string _sourceUrl = "https://pastebin.com/raw/JucRNpWs";

        public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
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

                var productsJson = JsonConvert.SerializeObject(products);
                _logger.LogInformation($"Received products: {productsJson}");
                //possible to use System.Text.Json or Newtonsoft.Json to convert to json.
                //note: in the task was "Appropriate logging including the full response".
                //But it is not recommended to log entire data in logs, because the file can growth fast.
                //So it's better to log count of items, or id's.
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error: {ex.Message}");
            }

            return products;
        }

        public IEnumerable<Product>? GetFilteredProducts(IEnumerable<Product>? products, FilterDto filter)
        {
            var filteredProducts = products?.AsQueryable() ?? Enumerable.Empty<Product>(); 
            //better to use queryable, because it creates query and only then apply it into source

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

            if (filter.WordsFromHighlight?.Any() ?? false)
            {
                if (filter.MostCommonWords?.Intersect(filter.WordsFromHighlight).Any() ?? false)
                {
                    filteredProducts = filteredProducts?.Where(x 
                        => x.Description
                            .ToLower()
                            .Split(Constants.Delimiters, StringSplitOptions.RemoveEmptyEntries)
                            .Intersect(filter.WordsFromHighlight)
                            .Any());
                }
            }

            _logger.LogInformation($"Filtered count of products: {filteredProducts?.Count()}");

            return filteredProducts;
        }

        public IEnumerable<ProductResponseDto>? Map(IEnumerable<Product>? products, string? highlight)
        {
            var wordsFromHighlight = highlight?.Split(",") ?? Enumerable.Empty<string>();

            var mappedProducts = products?.Select(ProductResponseDto.MapFrom).ToList();

            foreach (var mappedProduct in mappedProducts ?? Enumerable.Empty<ProductResponseDto>())
            {
                var words = mappedProduct.Description
                    .ToLower()
                    .Split(Constants.Delimiters, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                foreach (var highlightWord in wordsFromHighlight)
                {
                    var index = words.FindIndex(x => x == highlightWord);
                    if (index != -1)
                    {
                        mappedProduct.Description = mappedProduct.Description.Replace(highlightWord, $"<em>{highlightWord}</em>");
                    }
                }
            }

            return mappedProducts;
        }

        public async Task<IEnumerable<string>> GetCommonWords(int skip, int take)
        {
            var wordCounts = new Dictionary<string, int>();
            var allProducts = await GetAllProductsAsync();

            foreach (var product in allProducts ?? Enumerable.Empty<Product>())
            {
                var words = product.Description
                    .ToLower()
                    .Split(Constants.Delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    if (wordCounts.ContainsKey(word))
                    {
                        wordCounts[word]++;
                    }
                    else
                    {
                        wordCounts[word] = 1;
                    }
                }
            }

            var ordered = wordCounts.OrderByDescending(x => x.Value);

            var commonWords = ordered
                .Skip(skip)
                .Take(take)
                .Select(x => x.Key);

            var commonWordsJson = JsonConvert.SerializeObject(commonWords);
            _logger.LogInformation($"Common words, skip top {skip}, then take top {take}: {commonWordsJson}");

            return commonWords;
        }
    }
}
