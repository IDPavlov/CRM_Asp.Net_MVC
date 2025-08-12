using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Route("{controller}")]
    public class DealsController : Controller
    {
        private readonly CrmDbContext db;

        public DealsController(CrmDbContext db)
        {
            this.db = db;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var deals = await db.Deals
                .Include(d => d.Manager)     // Подгружаем связанные данные
                .Include(d => d.Client)
                .Include(d => d.Status)
                .Include(d => d.Product)     // Если есть связь с продуктом
                .ToListAsync();

            return View(deals);  // Передаём список сделок в представление
        }

        [HttpGet]
        [Route("{action}")]
        public IActionResult Create()
        {
            // Загружаем списки для выпадающих меню
            LoadDropdownData();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{action}")]
        public async Task<IActionResult> Create(CreateDealDto dto)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Ошибка: {error.ErrorMessage}");
            }

            if (ModelState.IsValid)
            {
                var deal = new Deal()
                {
                    Amount = dto.Amount,
                    ClientId = dto.ClientId,
                    StatusId = dto.StatusId,
                    ManagerId = dto.ManagerId,
                    ProductId = dto.ProductId,
                    Date = DateTime.UtcNow
                };
                db.Deals.Add(deal);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                LoadDropdownData();
                return View(dto);
            }
        }

        private void LoadDropdownData()
        {
            ViewBag.Statuses = db.DealStatuses.ToList();
            ViewBag.Managers = db.Managers.ToList();
            ViewBag.Clients = db.Clients.ToList();
            ViewBag.Products = db.Products.ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{action}/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deal = await db.Deals
                .FirstOrDefaultAsync(c => c.Id == id);

            if (deal == null) return NotFound();

            db.Deals.Remove(deal);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
