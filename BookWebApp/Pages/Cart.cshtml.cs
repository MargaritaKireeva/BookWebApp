using CartService.Models;
using CartService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWebApp.Pages
{
    public class CartModel : PageModel
    {
        private readonly CartServiceClient _cartService; // ������ ��� ������ � ��������

        public CartModel(CartServiceClient cartService)
        {
            _cartService = cartService;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // ������ ��������� � �������

        public async Task<IActionResult> OnGetAsync()
        {
            var cart = await _cartService.GetCartAsync();
            // �������� �������� ������� ��� �������� ������������
            CartItems = await _cartService.GetCartItemsAsync(cart.Id); // ����� ��������� ��������� �������
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
    }
}
