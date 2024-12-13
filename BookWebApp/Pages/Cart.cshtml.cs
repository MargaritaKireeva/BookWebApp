using CartService.Models;
using CartService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWebApp.Pages
{
    public class CartModel : PageModel
    {
        private readonly CartServiceClient _cartService; // Сервис для работы с корзиной

        public CartModel(CartServiceClient cartService)
        {
            _cartService = cartService;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Список элементов в корзине

        public async Task<IActionResult> OnGetAsync()
        {
            var cart = await _cartService.GetCartAsync();
            // Получаем элементы корзины для текущего пользователя
            CartItems = await _cartService.GetCartItemsAsync(cart.Id); // Метод получения элементов корзины
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(CartItem item)
        {
           // item.Book = null;
            // Логика обновления количества товара в корзине
            await _cartService.UpdateCartItemQuantityAsync(item);
            return RedirectToPage(); // Перенаправляем на ту же страницу после обновления
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int itemId)
        {
            // Логика удаления товара из корзины
            await _cartService.RemoveCartItemAsync(itemId);
            return RedirectToPage(); // Перенаправляем на ту же страницу после удаления
        }
    }
}
