using CartService.Consumers;
using CartService.Data;
using CartService.Repositories;
using CartService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService.Services.CartService>();
builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CartConnection")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("amqps://vqzxgtrq:C7UGMO30btVKqLFcDRip2S4Su1g7C2KK@kebnekaise.lmq.cloudamqp.com/vqzxgtrq"), h =>
        {
            h.Username("vqzxgtrq");
            h.Password("C7UGMO30btVKqLFcDRip2S4Su1g7C2KK");
        }); // Укажите адрес вашего RabbitMQ сервера
        cfg.ConfigureEndpoints(context); // Автоматическая конфигурация конечных точек
        cfg.ReceiveEndpoint("order-queue-cart", e =>
        {
            e.ConfigureConsumer<OrderCreatedCartConsumer>(context);
        });
    });

    // Регистрация IRequestClient для BookRequestEvent
    x.AddRequestClient<BookRequestEvent>();
    x.AddRequestClient<CartRequestEvent>();
    x.AddConsumer<CartRequestConsumer>();
    x.AddConsumer<UpdateBookConsumer>();
    x.AddConsumer<OrderCreatedCartConsumer>();

    // Регистрация EventBusRabbitMQ
    x.AddScoped<IEventBus, EventBusRabbitMQ>();

});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
