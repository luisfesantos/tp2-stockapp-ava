using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        public int Id { get; set; }zz
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
