using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System.Collections.Generic;
namespace OrderService.Data
{


    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        // DbSet для заказов
        public DbSet<Order> Orders { get; set; }

        // DbSet для элементов заказа
        public DbSet<OrderItems> OrderItems { get; set; }

        public DbSet<OutboxMessage> Outbox { get; set; }

    }
}
