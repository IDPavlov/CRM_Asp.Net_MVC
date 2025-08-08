using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    public class ClientsController : Controller
    {
        private readonly CrmDbContext db;

        public ClientsController(CrmDbContext db)
        {
            this.db = db;
        }

        // GET: Clients
        public async Task<IActionResult> Index(
            string? sortOrder,
            string? searchString)
        {
            ViewData["NameSort"] = (sortOrder == "name") ? "name_desc" : "name";
            ViewData["DateSort"] = (sortOrder == "date") ? "date_desc" : "date";

            var clients = db.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Name.Contains(searchString));
            }

            clients = sortOrder switch
            {
                "name" => clients.OrderBy(c => c.Name),
                "name_desc" => clients.OrderByDescending(c => c.Name),
                "date" => clients.OrderBy(c => c.RegistrationDate),
                "date_desc" => clients.OrderByDescending(c => c.RegistrationDate),
                _ => clients.OrderBy(c => c.Id)
            };

            return View(await clients.ToListAsync());
        }

        // GET: Clients/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await db.Clients
                .Include(c => c.Deals)          // Подгружаем сделки
                .Include(c => c.Interactions)   // Подгружаем взаимодействия
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await db.Clients
                .Include(c => c.Deals)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            if (client.Deals.Any(d => d.Status.Name != "Completed"))
            {
                ModelState.AddModelError("", "Нельзя удалить клиента с активными сделками!");
                return RedirectToAction(nameof(Index));
            }

            db.Clients.Remove(client);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
