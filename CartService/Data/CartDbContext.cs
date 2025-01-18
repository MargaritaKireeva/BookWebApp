using Microsoft.EntityFrameworkCore;
using CartService.Models;
using System.Collections.Generic;

namespace CartService.Data
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Outbox> OutBoxMessages { get; set; }
    }
}
