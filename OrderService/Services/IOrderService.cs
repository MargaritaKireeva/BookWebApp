using OrderService.Models;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int cartId);
        Task<List<Order>> GetOrdersByCartIdAsync(int cartId);
    }
}
