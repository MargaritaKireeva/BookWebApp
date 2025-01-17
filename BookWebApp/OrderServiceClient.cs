using CartService.Models;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace BookWebApp
{
    public class OrderServiceClient
    {
        private readonly HttpClient _client;

        public OrderServiceClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("HttpOrderService");
        }

        public async Task<Order> CreateOrder(int cartId)
        {
            var response = await _client.PostAsJsonAsync($"api/Orders/{cartId}", cartId);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
           /* return orders.LastOrDefault(order => order.CartId == cartId);*/
        }

    }
}
