using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Entities
{
    public class ProductFilters
    {
        public string? Query { get; set; }
        public string? SortBy { get; set; } = "Name";
        public bool Descending { get; set; } = false;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
