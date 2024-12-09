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

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = await GetCartByIdAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        // Добавляем методы для управления элементами корзины
        public async Task AddItemToCartAsync(int cartId, CartItem item)
        {
            var cart = await GetCartByIdAsync(cartId);
            if (cart != null)
            {
                item.CartId = cartId; // Устанавливаем идентификатор корзины
                await _context.CartItems.AddAsync(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateItemInCartAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
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
