using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class UpdateBookEvent
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        //public decimal Price { get; set; }
    }
}
