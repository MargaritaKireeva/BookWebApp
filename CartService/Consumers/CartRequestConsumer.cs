using MassTransit;
using Shared.Events;
using Shared;
using CartService.Services;

namespace CartService.Consumers
{
    public class CartRequestConsumer : IConsumer<CartRequestEvent>
    {
        private readonly ICartService _cartService; // Используем сервис вместо репозитория

        public CartRequestConsumer(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task Consume(ConsumeContext<CartRequestEvent> context)
        {
            var cartId = context.Message.CartId;
            var cartItems = await _cartService.GetItemsInCartAsync(cartId);
            CartResponse cartResponse = new CartResponse
            {
                Items = cartItems.Select(item => new CartItems
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Book.Price
                }).ToList()
            };
            // Отправка ответа обратно
            await context.RespondAsync(cartResponse);
        }
    }
}
