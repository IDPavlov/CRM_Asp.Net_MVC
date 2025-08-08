using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    public class DealsController : Controller
    {
        private readonly CrmDbContext db;

        public DealsController(CrmDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View(db.Deals.ToList<Deal>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Загружаем списки для выпадающих меню
            LoadDropdownData();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
