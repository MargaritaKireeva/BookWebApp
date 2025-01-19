using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookService.Data;
using BookService.Models;
using Shared.Events;
using Shared;
using System.Text.Json;

namespace BookService.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Books.Update(book);
                var updateBookInfo = new UpdateBookEvent
                {
                    BookId = book.Id,
                    Quantity = book.Quantity
                };
                var outBoxMessage = new OutboxMessage { Content = JsonSerializer.Serialize(updateBookInfo), Status = "Pending" };
                await _context.Outbox.AddAsync(outBoxMessage);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            //await _eventBus.Publish(updateBookInfo);
        }

        public async Task DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
