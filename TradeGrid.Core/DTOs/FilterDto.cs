namespace TradeGrid.Core.DTOs
{
    public class FilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public IEnumerable<string>? Sizes { get; set; } 
        public IEnumerable<string>? Words { get; set; }

        public FilterDto()
        {
            
        }

        public FilterDto(decimal? minPrice, decimal? maxPrice, string? size, string? highlight)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;

            Sizes = size?.Split(",");
            Words = highlight?.Split(",");
        }
    }
}
