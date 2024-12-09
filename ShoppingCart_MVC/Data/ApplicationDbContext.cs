using Microsoft.EntityFrameworkCore;
using ShoppingCart_MVC.Models;

namespace ShoppingCart_MVC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Item> Items { get; set; }
}