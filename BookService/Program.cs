using BookService.Consumers;
using BookService.Data;
using BookService.Repositories;
using BookService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService.Services.BookService>();
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BooksConnection")));

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
    x.AddRequestClient<BookRequestEvent>();
    x.AddConsumer<BookRequestConsumer>(); // Регистрация consumer для обработки запросов на книгу
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
