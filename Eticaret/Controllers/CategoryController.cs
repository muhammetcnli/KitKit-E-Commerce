using Microsoft.AspNetCore.Mvc;
using Eticaret.Models;
using Eticaret.Services;
using System.Linq;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    // Other actions like Create, Edit, Delete can be added here
}