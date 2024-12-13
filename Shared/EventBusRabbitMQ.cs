using System;
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
    public interface IEventBus
    {
        Task<BookResponse> RequestBook(BookRequestEvent request);
    }

    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRequestClient<BookRequestEvent> _requestClient;

        public EventBusRabbitMQ(IRequestClient<BookRequestEvent> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<BookResponse> RequestBook(BookRequestEvent request)
        {
            var response = await _requestClient.GetResponse<BookResponse>(request);
            return response.Message;
        }
    }
}

