using BookWebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
/*builder.Services.AddControllersWithViews();*/
/*builder.Services.AddRazorRuntimeCompilation();*/
builder.Services.AddHttpClient("HttpBookService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7048/"); // Укажите адрес BookService
});

builder.Services.AddScoped<BookServiceClient>();

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



/*app.MapControllers();
app.UseDeveloperExceptionPage();*/

app.MapRazorPages();


app.Run();
