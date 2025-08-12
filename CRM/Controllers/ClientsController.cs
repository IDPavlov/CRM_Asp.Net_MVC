using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Route("{controller}")]
    public class ClientsController : Controller
    {
        private readonly CrmDbContext db;

        public ClientsController(CrmDbContext db)
        {
            this.db = db;
        }

        [Route("{sortOrder?}/{searchString?}")]
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

        [HttpGet]
        [Route("{action}")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{action}")]
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

        [Route("{action}/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var client = await db.Clients
                .Include(c => c.Interactions)
                .Include(c => c.Deals)
                    .ThenInclude(d => d.Status)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) 
                return NotFound();
            else
                return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{action}/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await db.Clients
                .Include(c => c.Deals)
                    .ThenInclude(d => d.Status)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) 
                return NotFound();

            if (client.Deals.Any(d => d.Status.Name != "Completed"))
            {
                ModelState.AddModelError("", "Нельзя удалить клиента с активными сделками!");
                return RedirectToAction(nameof(Index));
            }

            db.Clients.Remove(client);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("{action}/{id}")]
        public async Task<IActionResult> Edit(int id)
        {

            Client? client = await db.Clients.FirstOrDefaultAsync(p => p.Id == id);
            if (client != null) 
                return View(client);
            else
                return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{action}")]
        public async Task<IActionResult> Edit(Client client)
        {
            db.Clients.Update(client);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
