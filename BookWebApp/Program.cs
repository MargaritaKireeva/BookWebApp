using BookWebApp;

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
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddScoped<BookServiceClient>();
builder.Services.AddScoped<CartServiceClient>();


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
