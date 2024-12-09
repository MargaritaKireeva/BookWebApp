using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Models;
using CartService.Repositories;

namespace CartService.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            return await _cartRepository.GetCartByIdAsync(cartId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _cartRepository.AddCartAsync(cart); 
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
            return await _cartRepository.GetItemsInCartAsync(cartId); 
        }
    }
}
