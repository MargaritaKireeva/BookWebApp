using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; } 
        public List<OrderItemInfo> OrderItems { get; set; }

        public class OrderItemInfo
        {
            public int BookId { get; set; }
            public int Quantity { get; set; } 
        }
    }
}
