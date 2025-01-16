﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Shared.Events;

namespace Shared
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }



    public class CartResponse
    {
        public List<CartItems> Items { get; set; } // Список элементов корзины
    }

    public interface IEventBus
    {
        Task<BookResponse> RequestBook(BookRequestEvent request);
        Task<CartResponse> RequestCartItems(CartRequestEvent request);
    }

    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRequestClient<BookRequestEvent> _requestClient;
        private readonly IRequestClient<CartRequestEvent> _cartRequestClient;

        public EventBusRabbitMQ(IRequestClient<BookRequestEvent> requestClient, IRequestClient<CartRequestEvent> cartRequestClient)
        {
            _requestClient = requestClient;
            _cartRequestClient = cartRequestClient;
        }

        public async Task<BookResponse> RequestBook(BookRequestEvent request)
        {
            var response = await _requestClient.GetResponse<BookResponse>(request);
            return response.Message;
        }

        public async Task<CartResponse> RequestCartItems(CartRequestEvent request)
        {
            var response = await _cartRequestClient.GetResponse<CartResponse>(request);
            return response.Message;
        }
    }
}

