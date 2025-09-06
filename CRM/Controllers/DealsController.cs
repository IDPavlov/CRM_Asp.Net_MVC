using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                .Include(d => d.Manager)
                .Include(d => d.Client)
                .Include(d => d.Status)
                .Include(d => d.DealProducts)
                    .ThenInclude(dp => dp.Product)
                .ToListAsync();

            return View(deals);  // Передаём список сделок в представление
        }

        [HttpGet]
        [Route("{action}")]
        public IActionResult Create()
        {
            // Загружаем списки для выпадающих меню
            LoadDropdownData();
            CreateDealDto dto = new();
            
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{action}")]
        public async Task<IActionResult> Create(CreateDealDto dto)
        {

            if (ModelState.IsValid)
            {
                var deal = new Deal()
                {
                    ClientId = dto.ClientId,
                    StatusId = dto.StatusId,
                    ManagerId = dto.ManagerId,
                };

                db.Deals.Add(deal);

                //сразу сохраняем, чтобы узнать Id сделки (deal)
                await db.SaveChangesAsync();

                if (dto.DealType == "service")
                {
                    deal.ServicePrice = dto.ServicePrice;
                }
                else if (dto.DealType == "product" && !dto.DealProducts.Any())
                {
                    ModelState.AddModelError("", "Для товарной сделки нужно добавить хотя бы один товар");
                    LoadDropdownData();
                    return View(dto);
                }
                else
                {
                    foreach (var productDto in dto.DealProducts)
                    {
                        deal.DealProducts.Add(new DealProduct
                        {
                            DealId = deal.Id,
                            ProductId = productDto.ProductId,
                            Quantity = productDto.Quantity,
                            UnitPrice = await db.Products
                                .Where(p => p.Id == productDto.ProductId)
                                .Select(p => p.Price)
                                .FirstOrDefaultAsync()
                        });
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Ошибка: {error.ErrorMessage}");
                }
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
