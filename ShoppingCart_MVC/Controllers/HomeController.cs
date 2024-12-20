using Microsoft.AspNetCore.Mvc;
using ShoppingCart_MVC.Data;
using ShoppingCart_MVC.Helpers;
using ShoppingCart_MVC.Models;
using System.Diagnostics;

namespace ShoppingCart_MVC.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var items = _context.Items.ToList();
        return View(items);
    }

    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        //Get selected item
        var item = _context.Items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        //Get currect cart or create a new one
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

        //Check if item is already in the cart
        var cartItem = cart.FirstOrDefault(item => item.ItemId == id);

        if (cartItem != null)
        {
            cartItem.Quantity++;
        }
        else
        {
            cart.Add(new CartItem { ItemId = item.Id, Name = item.Name, Price = item.Price, Quantity = 1 });
        }

        //Save the cart back to session
        HttpContext.Session.SetObject("Cart", cart);
        return RedirectToAction("Index");

    }

    public IActionResult ViewCart()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
        return View(cart);
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        //Retrieve cart from session
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

        //Find the item to remove
        var itemToRemove = cart.FirstOrDefault(item => item.ItemId == id);
        if (itemToRemove != null)
        {
            cart.Remove(itemToRemove);
        }

        //Save the updated cart back to session
        HttpContext.Session.SetObject("Cart", cart);
        return RedirectToAction("ViewCart");

    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
