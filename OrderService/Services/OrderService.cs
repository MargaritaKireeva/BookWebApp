using OrderService.Models;
using OrderService.Repositories;
using Shared;
using Shared.Events;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _eventBus;

        public OrderService(IOrderRepository orderRepository, IEventBus eventBus)
        {
            _orderRepository = orderRepository;
            _eventBus = eventBus;
        }

        public async Task<Order> CreateOrderAsync(int cartId)
        {
/*            // Запрос элементов корзины
            var cartRequest = new CartRequestEvent { CartId = cartId };
            var cartResponse = await _eventBus.RequestCartItems(cartRequest);

            // Создание нового заказа на основе полученных данных о корзине
            var order = new Order
            {
                CartId = cartId,
                TotalAmount = cartResponse.Items.Sum(item => item.Price * item.Quantity),
                OrderDate = DateTime.UtcNow
            };*/
           var order =  await _orderRepository.AddAsync(cartId);
           /* var orderItems = new List<OrderItems>();
            foreach(var item in cartResponse.Items)
            {
                var orderItem = new OrderItems
                {
                    BookId = item.BookId,
                    OrderId = order.Id,
                    CartId= item.CartId,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                orderItems.Add(orderItem); 
                await _orderRepository.AddItemInOrderAsync(orderItem);
            }
            order.OrderItems = orderItems;*/
            return order;
        }

        public async Task<List<Order>> GetOrdersByCartIdAsync(int cartId)
        {
            return await _orderRepository.GetByCartIdAsync(cartId);
        }
    }
}
