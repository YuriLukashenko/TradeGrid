using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Core.Models;

namespace TestTask.Core.DTOs
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
