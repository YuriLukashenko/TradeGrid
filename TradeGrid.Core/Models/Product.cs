namespace TradeGrid.Core.Models
{
    public class Product
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; } //prices also could be int or double type, but better is to use decimal.
        public IEnumerable<string> Sizes { get; set; } = new List<string>(); //typically setup as empty list to avoid NullReference Exceptions.
        public string Description { get; set; } = string.Empty; //could be string?, but I prefer to init it like that 
    }
}
