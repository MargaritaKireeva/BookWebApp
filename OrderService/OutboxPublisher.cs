using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Data;
using Shared;
using Shared.Events;
using System;
using System.Text.Json;

namespace OrderService
{
    public class OutboxPublisher : BackgroundService
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        //private readonly IEventBus _eventBus;

        public OutboxPublisher(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
           // _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    /*var service = scope.ServiceProvider.GetRequiredService<IServiceScope>();*/
                    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                    var _eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                    var messages = await dbContext.Outbox
                        .Where(x => x.Status == "Pending")
                        .ToListAsync(stoppingToken);

                    foreach (var message in messages)
                    {
                        // Publish the message using MassTransit
                        var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message.Content);
                        await _eventBus.Publish(orderCreatedEvent);

                        // Update message status
                        message.Status = "Sent";
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
