﻿using CartService.Models;

namespace CartService.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartByIdAsync(int cartId);
        Task<Cart> AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task ClearCartAsync(int id);

        Task AddItemToCartAsync(int cartId, CartItem item);
        Task UpdateItemInCartAsync(CartItem item);
        Task RemoveItemFromCartAsync(int itemId);
        Task<List<CartItem>> GetItemsInCartAsync(int cartId);
        Task<List<CartItem>> GetCartItemsByBookId(int bookId);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(int itemId);
    }
}
