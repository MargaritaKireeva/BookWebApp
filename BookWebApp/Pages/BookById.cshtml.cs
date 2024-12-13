using BookService.Models;
using CartService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWebApp.Pages
{
    public class BookByIdModel : PageModel
    {
        private readonly BookServiceClient _bookService;
        private readonly CartServiceClient _cartService;
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        public BookByIdModel(BookServiceClient bookService, CartServiceClient cartService)
        {
            _bookService = bookService;
            _cartService = cartService;
        }
        public async Task OnGet(int id)
        {
            var book = await _bookService.GetBookAsync(id);
            Id = book.Id;
            Title = book.Title; 
            Author = book.Author;
            Price = book.Price;
            Quantity = book.Quantity;

        }

        
        public async Task<IActionResult> OnPostAddToCartAsync(int bookId, int quantity)
        {
            // Создание новой корзины или получение существующей
            var cart = await _cartService.GetCartAsync();
            
            if (quantity > Quantity)
            {
                // Возвращаем уведомление о том, что количество превышает доступное
                ModelState.AddModelError(string.Empty, "Нельзя добавить в корзину количество большее чем доступное.");
            }
                // Создание элемента корзины
                var cartItem = new CartItem
            {
                BookId = bookId,
                Quantity = quantity
            };

            // Добавление элемента в корзину
            await _cartService.AddItemToCartAsync(cart.Id, cartItem);

            TempData["SuccessMessage"] = "Книга успешно добавлена в корзину!";

            return RedirectToPage(new { id = bookId }); 
        }
    }
}
