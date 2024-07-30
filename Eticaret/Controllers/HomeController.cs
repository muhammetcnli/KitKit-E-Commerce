using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eticaret.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Ensure you have this namespace for DbContext operations

namespace Eticaret.Controllers;

public class HomeController : Controller
{
    private readonly Services.ApplicationDbContext _context;

    public HomeController(Services.ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString, string category)
{   
    var productsQuery = _context.Products.AsQueryable();

    if (!String.IsNullOrEmpty(searchString))
    {
        productsQuery = productsQuery.Where(p => p.Name!.ToLower().Contains(searchString.ToLower()));
    }
    if (!String.IsNullOrEmpty(category) && category != "0")
    {
        productsQuery = productsQuery.Where(p => p.CategoryId == int.Parse(category));
    }

    var products = await productsQuery.ToListAsync();

    var model = new ProductViewModel
    {
        Products = products,
        Categories = await _context.Categories.ToListAsync(),
        SelectedCategory = category
    };

    return View(model);
}

    public async Task<IActionResult> Admin(string searchString, string category)
    {   
        var productsQuery = _context.Products.AsQueryable();

        if (!String.IsNullOrEmpty(searchString))
        {
            productsQuery = productsQuery.Where(p => p.Name!.ToLower().Contains(searchString.ToLower()));
        }
        if (!String.IsNullOrEmpty(category) && category != "0")
        {
            productsQuery = productsQuery.Where(p => p.CategoryId == int.Parse(category));
        }

        var products = await productsQuery.ToListAsync();

        var model = new ProductViewModel
        {
            Products = products,
            Categories = await _context.Categories.ToListAsync(),
            SelectedCategory = category
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories == null || !categories.Any())
            {
                ViewBag.Categories = new SelectList(new List<Category>(), "CategoryId", "Name");
                ModelState.AddModelError("", "No categories available. Please add categories first.");
            }
            else
            {
                ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            }
        }
        catch (Exception ex)
        {
            ViewBag.Categories = new SelectList(new List<Category>(), "CategoryId", "Name");
            ModelState.AddModelError("", "An error occurred while fetching categories. Please try again later.");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product model, IFormFile imageFile)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(imageFile.FileName);
        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

        if (imageFile != null)
        {
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
            }
        }

        if (ModelState.IsValid)
        {
            if (imageFile != null)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            model.Image = randomFileName;
            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin");
        }
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
        return View(model);
    }

[HttpGet("Home/Edit/{id?}")]
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var entity = await _context.Products.FindAsync(id);
    if (entity == null)
    {
        return NotFound();
    }

    ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
    return View(entity);
}

[HttpPost("Home/Edit/{id}")]
public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
{
    if (id != model.ProductId)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            if (imageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".PNG" };
                var extension = Path.GetExtension(imageFile.FileName);
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = randomFileName;
            }

            _context.Update(model);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(model.ProductId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction("Admin");
    }

    ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
    return View(model);
}

private bool ProductExists(int id)
{
    return _context.Products.Any(e => e.ProductId == id);
}

 [HttpGet]
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var entity = await _context.Products.FindAsync(id);
    if (entity == null)
    {
        return NotFound();
    }

    return View("DeleteConfirm", entity);
}

[HttpPost, ActionName("DeleteConfirmed")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int ProductId)
{
    var entity = await _context.Products.FindAsync(ProductId);
    if (entity == null)
    {
        return NotFound();
    }

    

    _context.Products.Remove(entity);
    await _context.SaveChangesAsync();
    TempData["Message"] = "Ürün başarıyla silindi.";
    return RedirectToAction("Admin");
}
    
    public IActionResult EditProduct(List<Product> Products){
        foreach (var product in Products)
        {
            Repository.EditIsActive(product);
        }
        return RedirectToAction("Admin");
    }

[HttpPost]
public async Task<IActionResult> Purchase(int id)
{
    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
    if (product == null)
    {
        return NotFound();
    }
    return View(product);
}

[HttpPost]
public async Task<IActionResult> CompletePurchase(int ProductId, string Password)
{
    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == ProductId);
    if (product == null)
    {
        TempData["PurchaseMessage"] = "Ürün bulunamadı.";
        return RedirectToAction("Index");
    }

    if (Password == "muhammet") // Doğru şifreyi burada belirleyin
    {
        try
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["PurchaseMessage"] = "Satın alma başarılı oldu ve ürün kaldırıldı!";
        }
        catch (Exception ex)
        {
            // Log the exception (ex) here for debugging purposes
            TempData["PurchaseMessage"] = "Satın alma sırasında bir hata oluştu. Lütfen tekrar deneyin.";
        }
        return RedirectToAction("Index");
    }
    else
    {
        TempData["PurchaseMessage"] = "Hatalı şifre. Lütfen tekrar deneyin.";
        return RedirectToAction("Index");
    }
}
}