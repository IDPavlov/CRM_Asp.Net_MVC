using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Route("{controller}")]
    public class ProductsController : Controller
    {
        private readonly CrmDbContext db;

        public ProductsController(CrmDbContext db) 
        {
            this.db = db;
        }

        [Route("")]
        public async Task<IActionResult> Index(
            string? sortOrder,
            string? searchString)
        {
            ViewData["PriceSort"] = (sortOrder == "price") ? "price_desc" : "price";

            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(c => c.Name.Contains(searchString));
            }

            products = sortOrder switch
            {
                "price" => products.OrderBy(c => c.Price),
                "price_desc" => products.OrderByDescending(c => c.Price),
                _ => products.OrderBy(c => c.Id)
            };

            return View(await products.ToListAsync());
        }

        [HttpGet]
        [Route("{action}")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{action}")]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }
    }
}
