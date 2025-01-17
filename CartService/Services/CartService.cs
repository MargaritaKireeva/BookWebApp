using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Models;
using CartService.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;

namespace CartService.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IEventBus _eventBus;

        public CartService(ICartRepository cartRepository, IEventBus eventBus)
        {
            _cartRepository = cartRepository;
            _eventBus = eventBus;
        }

        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            return await _cartRepository.GetCartByIdAsync(cartId);
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            return await _cartRepository.AddCartAsync(cart); 
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task DeleteCartAsync(int id)
        {
            await _cartRepository.DeleteCartAsync(id); 
        }

        public async Task AddItemToCartAsync(int cartId, CartItem item)
        {
            await _cartRepository.AddItemToCartAsync(cartId, item); 
        }

        public async Task UpdateItemInCartAsync(CartItem item)
        {
            await _cartRepository.UpdateItemInCartAsync(item); 
        }

        public async Task RemoveItemFromCartAsync(int itemId)
        {
            await _cartRepository.RemoveItemFromCartAsync(itemId); 
        }

        public async Task<List<CartItem>> GetItemsInCartAsync(int cartId)
        {
            var cartItems = await _cartRepository.GetItemsInCartAsync(cartId);
            var books = new List<Book>();

            foreach (var item in cartItems)
            {
                // Отправляем запрос на получение книги через RabbitMQ
                var bookRequestEvent = new BookRequestEvent { BookId = item.BookId };
                var bookResponse = await _eventBus.RequestBook(bookRequestEvent); // Получаем ответ от BookService
                var book = new Book
                {
                    Id = bookResponse.Id,
                    Author = bookResponse.Author,
                    Price = bookResponse.Price,
                    Title = bookResponse.Title,
                    Quantity = bookResponse.Quantity
                };
                books.Add(book);
            }

            // Объединяем данные о книгах с элементами корзины
            foreach (var item in cartItems)
            {
                var book = books.FirstOrDefault(b => b.Id == item.BookId);
                if (book != null)
                {
                    item.Book = book; 
                }
            }

            return cartItems;
        }
        public async Task<List<CartItem>> GetCartItemsByBookId(int bookId)
        {
            return await _cartRepository.GetCartItemsByBookId(bookId);
        }

        public async Task UpdateCartItemAsync(CartItem item)
        {
            await _cartRepository.UpdateCartItemAsync(item);
        }

        public async Task RemoveCartItemAsync(int itemId)
        {
            await _cartRepository.RemoveCartItemAsync(itemId);
        }

    }
}
