using Microsoft.AspNetCore.Mvc;
using ShoppingCart_MVC.Data;
using ShoppingCart_MVC.Helpers;
using ShoppingCart_MVC.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
        UpdateCartCountInViewBag();
        var items = _context.Items.ToList();
        return View(items);
    }

    [HttpPost]
    public IActionResult AddToCart(int id, int quantity)
    {
        //Get selected item
        var item = _context.Items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        //Get correct cart or create a new one
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

        //Check if item is already in the cart
        var cartItem = cart.FirstOrDefault(item => item.ItemId == id);

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem { ItemId = item.Id, Name = item.Name, Price = item.Price, Quantity = quantity, ImageUrl = item.ImageUrl });
        }

        //Save the cart back to session
        HttpContext.Session.SetObject("Cart", cart);
        return RedirectToAction("Index");

    }

    public IActionResult ViewCart()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
        //Update quantity
        ViewBag.CartCount = cart.Sum(c => c.Quantity);
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

    [HttpPost]
    public IActionResult UpdateCart(int id, int quantity)
    {
        //Retrieve the cart from the session
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

        //Find item to update
        var item = cart.FirstOrDefault(c => c.ItemId == id);

        if (item != null)         
        { 
            //Update quantity
            item.Quantity = quantity;

            //Save updated cart to session
            HttpContext.Session.SetObject("Cart", cart);

        }
        return RedirectToAction("ViewCart");
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        return View("Checkout page loaded");
    }

    [HttpGet]
    public IActionResult OrderConfirmation()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SubmitOrder(CheckoutViewModel model)
    {
        if (ModelState.IsValid)
        {
            TempData["SuccessMessage"] = "Your order has been placed successfully!";
            return RedirectToAction("OrderConfirmation");
        }
        return View("Checkout", model);
    }

    private void UpdateCartCountInViewBag()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
        if (cart != null)
        {
            ViewBag.CartCount = cart.Sum(item => item.Quantity);
        }
        else 
        { 
            ViewBag.CartCount = 0; 
        }
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
