using BookService.Models;
using BookService.Services;
using MassTransit;
using Shared.Events;

namespace BookService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IBookService _bookService;

        public OrderCreatedConsumer(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderCreatedEvent = context.Message;

            foreach (var item in orderCreatedEvent.OrderItems)
            {
                var updateBook = await _bookService.GetBookByIdAsync(item.BookId);
                updateBook.Quantity -= item.Quantity;
                await _bookService.UpdateBookAsync(updateBook);
            }
        }
    }
}
