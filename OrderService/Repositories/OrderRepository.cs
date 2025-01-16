using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repositories
{


    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderItems> AddItemInOrderAsync(OrderItems item)
        {
            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<Order>> GetByCartIdAsync(int cartId)
        {
            return await _context.Orders
                .Where(o => o.CartId == cartId)
                .Include(o => o.OrderItems) // Включаем элементы заказа
                .ToListAsync();
        }

        public async Task ClearOrdersAsync(int cartId)
        {
            var ordersToRemove = await _context.Orders
                .Where(o => o.CartId == cartId)
                .ToListAsync();

            _context.Orders.RemoveRange(ordersToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
