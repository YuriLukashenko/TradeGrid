namespace TradeGrid.Core.DTOs
{
    public class FilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public IEnumerable<string>? Sizes { get; set; } 
        public IEnumerable<string>? WordsFromHighlight { get; set; }

        public IEnumerable<string>? MostCommonWords { get; set; }

        public FilterDto()
        {
            
        }

        public FilterDto(decimal? minPrice, decimal? maxPrice, string? size, string? highlight, IEnumerable<string>? words)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;

            Sizes = size?.Split(",");
            WordsFromHighlight = highlight?.Split(",");

            MostCommonWords = words;
        }
    }
}
