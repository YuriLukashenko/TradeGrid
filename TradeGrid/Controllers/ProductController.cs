using Microsoft.AspNetCore.Mvc;
using TradeGrid.Core.DTOs;
using TradeGrid.Core.Interfaces;
using TradeGrid.Core.Models;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetProducts([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string? size, [FromQuery] string? highlight)
        {
            var products = Enumerable.Empty<Product>();

            if (HttpContext.Request.Query.Count == 0)
            {
                //another approach is to check every parameter equals null, but check count of query params can handle any number of parameters
                products = await _productService.GetAllProductsAsync();
            }
            else
            {
                var words = await _productService.GetCommonWords(5, 10);
                var filterDto = new FilterDto(minPrice, maxPrice, size, highlight, words);

                products = await _productService.GetFilteredProductsAsync(filterDto);
            }

            var mappedProducts = _productService.Map(products, highlight);

            return Ok(mappedProducts);
        }
    }
}
