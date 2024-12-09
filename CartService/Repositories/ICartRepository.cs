using CartService.Models;

namespace CartService.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByIdAsync(int cartId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(int id);
        Task AddItemToCartAsync(int cartId, CartItem item);
        Task UpdateItemInCartAsync(CartItem item);
        Task RemoveItemFromCartAsync(int itemId);
        Task<List<CartItem>> GetItemsInCartAsync(int cartId);
    }
}
