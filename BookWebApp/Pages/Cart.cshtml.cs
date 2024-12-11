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
            // �������� �������� ������� ��� �������� ������������
            CartItems = await _cartService.GetCartItemsAsync(); // ����� ��������� ��������� �������
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(CartItem item, int quantity)
        {
            // ������ ���������� ���������� ������ � �������
            await _cartService.UpdateCartItemQuantityAsync(item, quantity);
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
