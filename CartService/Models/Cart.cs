using System;
using System.Collections.Generic;

namespace CartService.Models
{
    public class Cart
    {
        public int Id { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<CartItem> Items { get; set; } = new List<CartItem>(); 
    }
}
