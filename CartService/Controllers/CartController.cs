using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Models;
using CartService.Services;

namespace CartService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<Cart>> GetUserCart(int cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> AddCart([FromBody] Cart cart)
        {
            return await _cartService.AddCartAsync(cart);
            /*return CreatedAtAction(nameof(GetUserCart), new { cartId = cart.Id }, cart);*/
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] Cart cart)
        {
            await _cartService.UpdateCartAsync(cart);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            await _cartService.DeleteCartAsync(id);
            return NoContent();
        }

        // Методы для управления элементами в корзине

        [HttpPost("{cartId}/Items")]
        public async Task<ActionResult> AddItemToCart(int cartId, [FromBody] CartItem item)
        {
            await _cartService.AddItemToCartAsync(cartId, item);
            return CreatedAtAction(nameof(GetUserItems), new { cartId }, item);
        }

        [HttpGet("{cartId}/items")]
        public async Task<ActionResult<List<CartItem>>> GetUserItems(int cartId)
        {
            var items = await _cartService.GetItemsInCartAsync(cartId);
            return Ok(items);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem([FromBody] CartItem item)
        {
            await _cartService.UpdateItemInCartAsync(item);
            return NoContent();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            await _cartService.RemoveItemFromCartAsync(itemId);
            return NoContent();
        }
    }
}
