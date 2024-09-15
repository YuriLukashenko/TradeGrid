using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeGrid.Core.Interfaces;
using TradeGrid.Core.Models;

namespace TradeGrid.Tests.ProductService
{
    public class GetAllProductTest
    {
        private readonly Mock<IProductService> _productServiceMock;

        public GetAllProductTest()
        {
            _productServiceMock = new Mock<IProductService>();
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new () { Title = "A Red Trouser", Price = 10 },
                new () { Title = "A Green Trouser", Price = 15 }
            };

            _productServiceMock.Setup(service => service.GetAllProductsAsync())
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productServiceMock.Object.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
