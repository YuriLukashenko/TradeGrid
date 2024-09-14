using TradeGrid.Core.Models;

namespace TradeGrid.Core.DTOs
{
    public class ProductResponseDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public IEnumerable<string> Sizes { get; set; } = new List<string>();
        public string Description { get; set; } = string.Empty;

        //it can be manual mapper or we can use AutoMapper library.
        public static ProductResponseDto MapFrom(Product product)
        {
            return new ProductResponseDto()
            {
                Title = product.Title,
                Price = product.Price,
                Sizes = product.Sizes,
                Description = product.Description,
            };
        }
    }
}
