using TradeGrid.Core.Models;

namespace TradeGrid.Core.DTOs
{
    public class ProductDto
    {
        public IEnumerable<Product>? Products { get; set; }
        public ApiKeys? ApiKeys { get; set; }
    }

    public class ApiKeys
    {
        public string? Primary { get; set; }
        public string? Secondary { get; set; }
    }
}
