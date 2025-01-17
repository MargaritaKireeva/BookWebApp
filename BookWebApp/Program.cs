using BookWebApp;
using MassTransit;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
/*builder.Services.AddControllersWithViews();*/
/*builder.Services.AddRazorRuntimeCompilation();*/
builder.Services.AddHttpClient("HttpBookService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7048/"); 
});

builder.Services.AddHttpClient("HttpCartService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7265/"); 
});
builder.Services.AddHttpClient("HttpOrderService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7266/");
});
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


    // Регистрация EventBusRabbitMQ
    x.AddScoped<IEventBus, EventBusRabbitMQ>();

});
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddScoped<BookServiceClient>();
builder.Services.AddScoped<CartServiceClient>();
builder.Services.AddScoped<OrderServiceClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();



/*app.MapControllers();
app.UseDeveloperExceptionPage();*/

app.MapRazorPages();


app.Run();
