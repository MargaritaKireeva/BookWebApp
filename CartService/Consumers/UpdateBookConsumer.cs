using CartService.Services;
using MassTransit;
using Shared.Events;

namespace CartService.Consumers
{
    public class UpdateBookConsumer : IConsumer<UpdateBookEvent>
    {
        private readonly ICartService _cartService;

        public UpdateBookConsumer(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task Consume(ConsumeContext<UpdateBookEvent> context)
        {
            var updateBookEvent = context.Message;
            var cartItems = await _cartService.GetCartItemsByBookId(updateBookEvent.BookId);

            foreach (var item in cartItems)
            {
                if (item.Quantity > updateBookEvent.Quantity)
                {
                    // Уменьшаем количество до максимального доступного
                    item.Quantity = updateBookEvent.Quantity;
                    
                }
                else if (updateBookEvent.Quantity <= 0)
                {
                    item.Quantity = 0;
                }
                await _cartService.UpdateCartItemAsync(item);
            }
            
        }
    }
}
