using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.Data;
using OrderService.Repositories;
using OrderService.Services;
using Shared;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService.Services.OrderService>();
//builder.Services.AddScoped<OutboxPublisher>();
builder.Services.AddHostedService<OutboxPublisher>();
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

    });

    x.AddRequestClient<CartRequestEvent>();

    // Регистрация EventBusRabbitMQ
    x.AddScoped<IEventBus, EventBusRabbitMQ>();

});
builder.Services.AddScoped<IEventBus, EventBusRabbitMQ>();
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
