using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using resorty.Data;
using resorty.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<resortyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("resortyContext") ?? throw new InvalidOperationException("Connection string 'resortyContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

// session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedUser.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); // session

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();
