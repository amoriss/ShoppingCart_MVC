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
        return View(new Item());
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

    [HttpGet]
    public IActionResult EditProduct(int id)
    {

        var product = _context.Items.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    public IActionResult EditProduct(Item item, IFormFile? NewImage)
    {
        if (ModelState.IsValid)
        {
            var existingProduct = _context.Items.Find(item.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            //Update product details
            existingProduct.Name = item.Name;
            existingProduct.Price = item.Price;

            //Image Upload
            if (NewImage != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + NewImage.FileName;

                //Save new image file
                using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                {
                    NewImage.CopyTo(fileStream);
                }

                //Update image URL
                existingProduct.ImageUrl = "/images/" + uniqueFileName;
            }


            //_context.Items.Update(item);

            //Save changes to the database
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(item);
    }

    [HttpPost]
    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Items.Find(id);
        if (product != null)
        {
            _context.Items.Remove(product);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
