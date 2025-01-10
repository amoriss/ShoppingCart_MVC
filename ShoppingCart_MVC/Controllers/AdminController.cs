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
            if (NewImage != null && NewImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                //Generate a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(NewImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                //Save new image file to server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    NewImage.CopyTo(fileStream);
                }

                //Update image URL
                existingProduct.ImageUrl = "/uploads/" + fileName;
            }


            //_context.Items.Update(item);

            //Save changes to the database
            _context.Items.Update(existingProduct);
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
