using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookService.Models;
using BookService.Repositories;

namespace BookService.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync(); // Возвращаем список книг
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id); // Возвращаем книгу 
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book); // Добавляем книгу 
        }
        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book); // Обновляем книгу напрямую.
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id); // Удаляем книгу напрямую.
        }
    }

}
