

using CartService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace BookWebApp
{
    public class CartServiceClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartServiceClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient("HttpCartService");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Cart> GetCartAsync()
        {
            // Получаем cartId из сессии
            int? id = _httpContextAccessor.HttpContext.Session.GetInt32("CartId");
            if (id == null)
            {
                var newCart = new Cart();
                
                
                newCart = await AddCart(newCart);
                id = newCart.Id;
                // Сохраняем cartId в сессии
                _httpContextAccessor.HttpContext.Session.SetInt32("CartId", (int)id);
            }
            var response = await _client.GetAsync($"api/cart/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Cart>();
        }

        public async Task<Cart> AddCart(Cart cart)
        {

            var response = await _client.PostAsJsonAsync("api/Cart", cart);
            response.EnsureSuccessStatusCode(); // Проверяем успешность ответа

            // Читаем ответ и возвращаем идентификатор новой корзины
            var newCart = await response.Content.ReadFromJsonAsync<Cart>();
            return newCart;
        }

        public async Task AddItemToCartAsync(int cartId, CartItem item)
        {
            var response = await _client.PostAsJsonAsync($"api/Cart/{cartId}/Items", item);
            response.EnsureSuccessStatusCode();
        }
        public async Task<List<CartItem>> GetCartItemsAsync(int cartId)
        {
            var response = await _client.GetAsync($"api/cart/{cartId}/items");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CartItem>>();
        }

        // Обновление количества товара в корзине
        public async Task UpdateCartItemQuantityAsync(CartItem item)
        {
            var response = await _client.PutAsJsonAsync($"api/Cart/items/{item.Id}", item);
            response.EnsureSuccessStatusCode();
        }

        // Удаление товара из корзины
        public async Task RemoveCartItemAsync(int itemId)
        {
            var response = await _client.DeleteAsync($"api/Cart/items/{itemId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
