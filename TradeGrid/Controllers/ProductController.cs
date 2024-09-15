using Microsoft.AspNetCore.Mvc;
using TradeGrid.Core.DTOs;
using TradeGrid.Core.Interfaces;
using TradeGrid.Core.Models;

namespace TradeGrid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
            //could be primary constructor, but I prefer old approach like this. 
        }

        [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [HttpGet("filter")]
        public async Task<IActionResult> GetProducts([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string? size, [FromQuery] string? highlight)
        {
            try
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

                    products = _productService.GetFilteredProducts(await _productService.GetAllProductsAsync(), filterDto);
                }

                var mappedProducts = _productService.Map(products, highlight);

                return Ok(mappedProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ProductController)}:{nameof(GetProducts)} throws an error: {ex.Message}");
                return BadRequest(new ErrorDto()
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
        }
    }
}