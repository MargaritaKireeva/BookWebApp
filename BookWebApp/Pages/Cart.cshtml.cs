using CartService.Models;
using CartService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWebApp.Pages
{
    public class CartModel : PageModel
    {
        private readonly CartServiceClient _cartService; // ������ ��� ������ � ��������
        private readonly OrderServiceClient _orderService;

        public CartModel(CartServiceClient cartService, OrderServiceClient orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // ������ ��������� � �������

        public async Task<IActionResult> OnGetAsync()
        {
            var cart = await _cartService.GetCartAsync();
            // �������� �������� ������� ��� �������� ������������
            CartItems = await _cartService.GetCartItemsAsync(cart.Id); // ����� ��������� ��������� �������
            foreach (var item in CartItems)
            {
                if (item.Quantity == 0) 
                {
                    ModelState.AddModelError(string.Empty, "������ ��� � �������");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(CartItem item)
        {
           // item.Book = null;
            // ������ ���������� ���������� ������ � �������
            await _cartService.UpdateCartItemQuantityAsync(item);
            return RedirectToPage(); // �������������� �� �� �� �������� ����� ����������
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int itemId)
        {
            // ������ �������� ������ �� �������
            await _cartService.RemoveCartItemAsync(itemId);
            return RedirectToPage(); // �������������� �� �� �� �������� ����� ��������
        }

        public async Task<IActionResult> OnPostCreateOrder()
        {

            // ������� �����
            var cart = await _cartService.GetCartAsync();
            var order = await _orderService.CreateOrder(cart.Id);

            // �������������� �� �������� ������������� ������ ��� ���������� ��������� �� ������
            return RedirectToPage("OrderConfirmation", new { orderId = order.Id });
        }
    }
}
