using Microsoft.Extensions.Logging;
using Moq;
using TradeGrid.Core.DTOs;
using TradeGrid.Core.Models;

namespace TradeGrid.Tests.ProductService
{
    public class FiltersTest
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<Services.ProductService>> _loggerMock;
        private readonly Services.ProductService _productService;

        public FiltersTest()
        {
            //mocking dependencies 
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };
            _loggerMock = new Mock<ILogger<Services.ProductService>>();

            //use real service with mocking dependencies
            _productService = new Services.ProductService(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public void GetFilteredProducts_ShouldFilterByMinPrice()
        {
            var products = new List<Product>
            {
                new () { Title = "A Red Trouser", Price = 10 },
                new () { Title = "A Green Trouser", Price = 15 },
                new () { Title = "A Blue Shirt", Price = 11 }
            };

            var filter = new FilterDto
            {
                MinPrice = 12
            };

            var result = _productService.GetFilteredProducts(products, filter);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(15, result.First().Price);
            // check that it is the "A Green Trouser" product
        }

        [Fact]
        public void GetFilteredProducts_ShouldFilterByMaxPrice()
        {
            var products = new List<Product>
            {
                new () { Title = "A Red Trouser", Price = 10 },
                new () { Title = "A Green Trouser", Price = 15 },
                new () { Title = "A Blue Shirt", Price = 11 }
            };

            var filter = new FilterDto
            {
                MaxPrice = 12
            };

            var result = _productService.GetFilteredProducts(products, filter);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // 2 products should be
        }

        [Fact]
        public void GetFilteredProducts_ShouldFilterBySizes()
        {
            var products = new List<Product>
            {
                new () { Title = "A Red Trouser", Price = 10, Sizes = new List<string> { "small", "medium" } },
                new () { Title = "A Green Trouser", Price = 15, Sizes = new List<string> { "large", "medium" } },
                new () { Title = "A Blue Shirt", Price = 11, Sizes = new List<string> { "small" } }
            };

            var filter = new FilterDto
            {
                Sizes = new List<string> { "small" }
            };

            var result = _productService.GetFilteredProducts(products, filter);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredProducts_ShouldFilterByMultipleCriteria()
        {
            var products = new List<Product>
            {
                new () { Title = "A Red Trouser", Price = 10, Sizes = new List<string> { "small", "medium" } },
                new () { Title = "A Green Trouser", Price = 15, Sizes = new List<string> { "large", "medium" } },
                new () { Title = "A Blue Shirt", Price = 11, Sizes = new List<string> { "small" } }
            };

            var filter = new FilterDto
            {
                MinPrice = 13,
                Sizes = new List<string> { "medium" }
            };

            var result = _productService.GetFilteredProducts(products, filter);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(15, result.First().Price);
        }
    }
}
