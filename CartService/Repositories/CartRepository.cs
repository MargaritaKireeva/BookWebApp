using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CartService.Data;
using CartService.Models;

namespace CartService.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;

        public CartRepository(CartDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            return await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return await _context.Carts
       .OrderByDescending(c => c.Id) 
       .FirstOrDefaultAsync();
        }


        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.Items);
                await _context.SaveChangesAsync();
            }
        }

                // Добавляем методы для управления элементами корзины
        public async Task AddItemToCartAsync(int cartId, CartItem item)
        { 
            var cart = await GetCartByIdAsync(cartId);
            if (cart != null)
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.BookId == item.BookId);

                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                    _context.Update(existingItem);
                }
                else
                {
                    cart.Items.Add(item);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateItemInCartAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartItemsByBookId(int bookId)
        {
            return await _context.CartItems.Where(ci => ci.BookId == bookId).ToListAsync();
        }

        public async Task UpdateCartItemAsync(CartItem item)
        {
            var existingItem = await _context.CartItems.FindAsync(item.Id);

            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                _context.CartItems.Update(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveCartItemAsync(int itemId)
        {
            var itemToRemove = await _context.CartItems.FindAsync(itemId);

            if (itemToRemove != null)
            {
                _context.CartItems.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveItemFromCartAsync(int itemId)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CartItem>> GetItemsInCartAsync(int cartId)
        {
            return await _context.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
        }
    }
}
