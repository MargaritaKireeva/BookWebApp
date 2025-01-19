using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookService.Data;
using Shared;
using Shared.Events;
using System;
using System.Text.Json;

namespace BookService
{
    public class OutboxPublisher : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OutboxPublisher(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>();
                    var _eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                    var messages = await dbContext.Outbox
                        .Where(x => x.Status == "Pending")
                        .ToListAsync(stoppingToken);

                    foreach (var message in messages)
                    {
                        // Publish the message using MassTransit
                        var updateBookEvent = JsonSerializer.Deserialize<UpdateBookEvent>(message.Content);
                        await _eventBus.Publish(updateBookEvent);

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
