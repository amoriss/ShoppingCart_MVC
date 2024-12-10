using Microsoft.EntityFrameworkCore;
using ShoppingCart_MVC.Data;
using ShoppingCart_MVC.Models;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
   new MySqlServerVersion(new Version(8,0,33))));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedDatabase(context);
}

app.Run();

static void SeedDatabase (ApplicationDbContext context)
{
    if(!context.Items.Any())
    {
        context.Items.AddRange(
            new Item { Name = "Product1", Price = 20.00},
            new Item { Name = "Product2", Price = 15.00},
            new Item { Name = "Product3", Price = 12.99},
            new Item { Name = "Product4", Price = 3.50}
            );

    }

}