namespace ShoppingCart_MVC.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public string? ImagePath { get; set; }
}
