using OrderService.Models;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(int cartId);
        Task<OrderItems> AddItemInOrderAsync(OrderItems item);
        Task<List<Order>> GetByCartIdAsync(int cartId);
        Task ClearOrdersAsync(int cartId);
    }
}
