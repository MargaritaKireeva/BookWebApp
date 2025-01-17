using CartService.Models;
using CartService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWebApp.Pages
{
    public class CartModel : PageModel
    {
        private readonly CartServiceClient _cartService; // Сервис для работы с корзиной
        private readonly OrderServiceClient _orderService;

        public CartModel(CartServiceClient cartService, OrderServiceClient orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Список элементов в корзине

        public async Task<IActionResult> OnGetAsync()
        {
            var cart = await _cartService.GetCartAsync();
            // Получаем элементы корзины для текущего пользователя
            CartItems = await _cartService.GetCartItemsAsync(cart.Id); // Метод получения элементов корзины
            foreach (var item in CartItems)
            {
                if (item.Quantity == 0) 
                {
                    ModelState.AddModelError(string.Empty, "Товара нет в наличии");
                }
            }
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

        public async Task<IActionResult> OnPostCreateOrder()
        {

            // Создаем заказ
            var cart = await _cartService.GetCartAsync();
            var order = await _orderService.CreateOrder(cart.Id);

            // Перенаправляем на страницу подтверждения заказа или отображаем сообщение об успехе
            return RedirectToPage("OrderConfirmation", new { orderId = order.Id });
        }
    }
}
