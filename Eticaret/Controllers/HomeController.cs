using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eticaret.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eticaret.Controllers;

public class HomeController : Controller
{

    public HomeController()
    {   

    }

    public IActionResult Index(string searchString, string category)
    {   
        var products = Repository.Products;

        if(!String.IsNullOrEmpty(searchString)){
            products = products.Where(p=>p.Name.ToLower().Contains(searchString)).ToList();
        }
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId","Name");
        return View(products);

        if(!String.IsNullOrEmpty(category)){
            products = products.Where(p=>p.CategoryId == int.Parse(category)).ToList();
        }
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId","Name");
        return View(products);
    }

    public IActionResult Admin()
    {
        return View(Repository.Products);
    }
}
