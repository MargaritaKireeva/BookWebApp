using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using Shared;
using Shared.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OrderService.Repositories
{


    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        private readonly IEventBus _eventBus;

        public OrderRepository(OrderDbContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public async Task<Order> AddAsync(int cartId)
        {
            // Запрос элементов корзины
            var cartRequest = new CartRequestEvent { CartId = cartId };
            var cartResponse = await _eventBus.RequestCartItems(cartRequest);

            // Создание нового заказа на основе полученных данных о корзине
            var order = new Order
            {
                CartId = cartId,
                TotalAmount = cartResponse.Items.Sum(item => item.Price * item.Quantity),
                OrderDate = DateTime.UtcNow
            };
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
               _context.Orders.Add(order);
                var orders = await GetByCartIdAsync(cartId);
                order = orders.LastOrDefault(order => order.CartId == cartId);

                var orderItems = new List<OrderItems>();
                foreach (var item in cartResponse.Items)
                {
                    var orderItem = new OrderItems
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        CartId = item.CartId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    orderItems.Add(orderItem);
                    await AddItemInOrderAsync(orderItem);
                }
                order.OrderItems = orderItems;
                var orderCreatedEvent = new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    OrderItems = order.OrderItems.Select(oi => new OrderCreatedEvent.OrderItemInfo
                    {
                        BookId = oi.BookId,
                        Quantity = oi.Quantity
                    }).ToList()
                };
                // Create an OutBox message
                var outBoxMessage = new OutboxMessage { Content = JsonSerializer.Serialize(orderCreatedEvent), Status = "Pending" };
                await _context.Outbox.AddAsync(outBoxMessage);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            /*await _eventBus.Publish(orderCreatedEvent);*/
            return order;
        }

        public async Task<OrderItems> AddItemInOrderAsync(OrderItems item)
        {
            _context.OrderItems.Add(item);
            //await _context.SaveChangesAsync();
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
