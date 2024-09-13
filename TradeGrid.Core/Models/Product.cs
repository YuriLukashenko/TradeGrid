namespace TestTask.Core.Models
{
    public class Product
    {
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; } //real prices also could be decimal or double type.
        public IEnumerable<string> Sizes { get; set; } = new List<string>(); //typically setup as empty list to avoid NullReference Exceptions.
        public string Description { get; set; } = string.Empty; //could be string?, but I prefer to init it like that 
    }
}
