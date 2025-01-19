using CartService.Services;
using MassTransit;
using Shared.Events;

namespace CartService.Consumers
{
    public class OrderCreatedCartConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ICartService _cartService;

        public OrderCreatedCartConsumer(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderCreatedEvent = context.Message;
            await _cartService.ClearCartAsync(orderCreatedEvent.CartId);

        }
    }
}
