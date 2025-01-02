using Microsoft.AspNetCore.Mvc;
using ShoppingCart_MVC.Data;
using ShoppingCart_MVC.Models;
using System.Data.SqlTypes;

namespace ShoppingCart_MVC.Controllers;
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var products = _context.Items.ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddProduct(Item item)
    {
        if (ModelState.IsValid)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(item);
    }
}
