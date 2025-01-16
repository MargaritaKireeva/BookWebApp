using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // Метод для создания заказа
    [HttpPost]
    public async Task<IActionResult> CreateOrder(int cartId)
    {
  /*      if (orderItems == null || !orderItems.Any())
        {
            return BadRequest("Корзина пуста.");
        }*/

        var order = await _orderService.CreateOrderAsync(cartId);

        return CreatedAtAction(nameof(GetOrders), new { cartId }, order);
    }

    // Метод для получения заказов по идентификатору корзины
    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetOrders(int cartId)
    {
        var orders = await _orderService.GetOrdersByCartIdAsync(cartId);

        if (orders == null || !orders.Any())
        {
            return NotFound();
        }

        return Ok(orders);
    }
}
