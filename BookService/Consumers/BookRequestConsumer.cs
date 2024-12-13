using BookService.Services;
using MassTransit;
using Shared;
using Shared.Events;

namespace BookService.Consumers
{
    public class BookRequestConsumer : IConsumer<BookRequestEvent>
    {
        private readonly IBookService _bookService; // Используем сервис вместо репозитория

        public BookRequestConsumer(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task Consume(ConsumeContext<BookRequestEvent> context)
        {
            var bookId = context.Message.BookId;
            var book = await _bookService.GetBookByIdAsync(bookId); // Получаем книгу через сервис
            BookResponse bookResponse = new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Quantity = book.Quantity
            };
            // Отправка ответа обратно в CartService
            await context.RespondAsync(bookResponse);
        }
    }
}
